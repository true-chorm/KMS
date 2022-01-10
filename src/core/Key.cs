
namespace KMS.src.core
{
    /// <summary>
    /// 2021-01-07 17:17
    /// </summary>
    class Key
    {
        /// <summary>
        /// 在数据库中存储的类型值。具体请参考《软件设计.md》
        /// </summary>
        internal Type Type
        {
            get;
            set;
        }

        internal int Code
        {
            get
            {
                return Type.Code;
            }
        }

        internal string Name
        {
            get
            {
                return Type.Desc;
            }
        }

        internal string DisplayName
        {
            get;
            set;
        }

        internal Key(int typeCode, string typeDesc) : this(typeCode, typeDesc, null)
        {

        }

        internal Key(int typeCode, string typeDesc, string displayName) : this(new Type(typeCode, typeDesc), displayName)
        {
            
        }

        internal Key(Type type, string displayName)
        {
            Type = type;
            DisplayName = displayName;
        }
    }
}
