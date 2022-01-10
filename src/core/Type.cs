namespace KMS.src.core
{
    /// <summary>
    /// 描述存储到数据库中去的记录类型。具体请参考《软件设计.md》
    /// </summary>
    class Type
    {
        internal int Code;
        internal string Desc;

        internal Type(int code, string desc)
        {
            Code = code;
            Desc = desc;
        }
    }
}