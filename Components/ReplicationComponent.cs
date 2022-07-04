using HECSFramework.Core;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Components
{

    public abstract class ReplicationComponent : BaseComponent
    {
        [Flags]
        protected enum FieldMask : int
        {
            Field_0 = 1 << 0,
            Field_1 = 1 << 1,
            Field_2 = 1 << 2,
            Field_3 = 1 << 3,
            Field_4 = 1 << 4,
            Field_5 = 1 << 5,
            Field_6 = 1 << 6,
            Field_7 = 1 << 7,
            Field_8 = 1 << 8,
            Field_9 = 1 << 9,
            Field_10 = 1 << 10,
            Field_11 = 1 << 11,
            Field_12 = 1 << 12,
            Field_13 = 1 << 13,
            Field_14 = 1 << 14,
            Field_15 = 1 << 15
        }


        [AttributeUsage(AttributeTargets.Field)]
        protected class ReplicationFieldAttribute : Attribute
        {

            private object fieldParent;
            private FieldInfo fieldInfo;
            private object replicationToken;

            public FieldMask Mask { get; private set; }

            //TODO Gives wrong boolean size
            public int FieldSize { get; private set; }
            public Type FieldType { get; private set; }

            //TODO Get rid of boxing, unboxing structures
            public object GetValue() => fieldInfo.GetValue(fieldParent);
            public void SetValue(object value) => fieldInfo.SetValue(fieldParent, value);

            public bool IsDirty
            {
                get
                {
                    return !replicationToken.Equals(fieldInfo.GetValue(fieldParent));
                }
            }

            private bool EqualsValue(IntPtr a, IntPtr b, int length)
            {
                for (int i = 0; i < length; i++)
                {
                    if (Marshal.ReadByte(a, i) != Marshal.ReadByte(b, i)) return false;
                }
                return true;
            }
            public ReplicationFieldAttribute(FieldMask mask)
            {
                Mask = mask;
            }



            internal unsafe void Init(ReplicationComponent replicationComponent, FieldInfo fieldInfo)
            {
                fieldParent = replicationComponent;
                this.fieldInfo = fieldInfo;

                FieldSize = Marshal.SizeOf(fieldInfo.FieldType);
                FieldType = fieldInfo.FieldType;

                replicationToken = GetValue();

           

                //Marshal.PtrToStructure(fieldInfo.FieldHandle.Value, fieldInfo.FieldType);
                //fieldInfo.GetValueDirect(TypedReference.MakeTypedReference);
            }
            ~ReplicationFieldAttribute()
            {
                //   Marshal.FreeHGlobal(replicationToken);
            }
        }

      

        private Dictionary<FieldMask, ReplicationFieldAttribute> replicationMap = new Dictionary<FieldMask, ReplicationFieldAttribute>();


        public int ComponentID => GetTypeHashCode;
        public byte[] ReplicationData { get; private set; } = null;
        public bool IsDirty => replicationMap.Values.Any((f) => f.IsDirty);


        public int DirtyDataSize => replicationMap.Values.Sum((f) =>
        {
            if (f.IsDirty) return f.FieldSize;
            return 0;
        }) + sizeof(FieldMask);

        private int GetDataSizeByMask(FieldMask mask) => sizeof(int) + replicationMap.Values.Sum((f) =>
        {
            if (mask.HasFlag(f.Mask)) return f.FieldSize;
            return 0;
        }) + sizeof(FieldMask);

        public ReplicationComponent()
        {
            FieldInfo[] fields = GetType().GetFields();
            foreach (FieldInfo field in fields)
            {
                ReplicationFieldAttribute replicationField = field.GetCustomAttribute<ReplicationFieldAttribute>();
                if (replicationField != null)
                {
                    if (replicationMap.ContainsKey(replicationField.Mask)) throw new Exception($"Reusing mask: '{replicationField.Mask}' on field: '{GetType()}.{field.Name}'");
                    replicationField.Init(this, field);
                    replicationMap.Add(replicationField.Mask, replicationField);
                }
            }
            replicationMap.OrderBy(kv => kv.Key);
        }


        private unsafe byte[] GetReplicationData(FieldMask bitMask)
        {
            byte[] data = new byte[GetDataSizeByMask(bitMask)];
            fixed (byte* ptr = ReplicationData)
            {
                int index = 0;

                int size = sizeof(int);
                int hashCodeType = GetTypeHashCode;
                Buffer.MemoryCopy(&hashCodeType, ptr + index, size, size);
                index += size;

                size = sizeof(FieldMask);
                Buffer.MemoryCopy(&bitMask, ptr + index, size, size);
                index += size;

                foreach (var replicationField in replicationMap.Values)
                {
                    if (!bitMask.HasFlag(replicationField.Mask)) continue;
                    size = replicationField.FieldSize;

                    object value = replicationField.GetValue();
                    var handler = GCHandle.Alloc(value, GCHandleType.Pinned);
                    var ptrValue = handler.AddrOfPinnedObject();

                    Buffer.MemoryCopy(ptrValue.ToPointer(), ptr + index, size, size);

                    handler.Free();
                    index += size;
                }
            }
            return data;
        }

        internal unsafe void UpdateData(byte[] componentData)
        {
            fixed (byte* ptr = componentData)
            {
                int index = 0;

                int size = sizeof(int);
                int hashCodeType = 0;
                Buffer.MemoryCopy(ptr + index, &hashCodeType, size, size);
                index += size;

                size = sizeof(FieldMask);
                FieldMask bitMask = 0;
                Buffer.MemoryCopy(ptr + index, &bitMask, size, size);
                index += size;

                foreach (var replicationField in replicationMap.Values)
                {
                    if (!bitMask.HasFlag(replicationField.Mask)) continue;
                    size = replicationField.FieldSize;


                    replicationField.SetValue(Marshal.PtrToStructure((IntPtr)(ptr + index), replicationField.FieldType));
                  
                    index += size;
                }
            }
        }

        internal byte[] GetFullData()
        {
            return GetReplicationData(GetDirtyBitMask());
        }
        private FieldMask GetDirtyBitMask()
        {
            FieldMask bitMask = 0;

            foreach (var replicationField in replicationMap.Values)
            {
                if (replicationField.IsDirty) bitMask |= replicationField.Mask;
            }
            return bitMask;
        }
        private FieldMask GetFullBitMask()
        {
            FieldMask bitMask = 0;

            foreach (var replicationField in replicationMap.Values)
            {
                bitMask |= replicationField.Mask;
            }
            return bitMask;
        }
    }
}
