using System;
using System.IO;
using System.Runtime.Serialization;

public   class Serialization
    {
    public static byte[] Serialize(object obj)
    {
        MemoryStream stream = new MemoryStream();
        DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
        serializer.WriteObject(stream, obj);
        byte[] bytes= stream.GetBuffer();
        stream.Close();
        return bytes;
    }
    public static T DeserializeM<T>(byte[] bytes)
    {
        MemoryStream stream = new MemoryStream(bytes);
        DataContractSerializer serializer = new DataContractSerializer(typeof(T));
        stream.Seek(0, SeekOrigin.Begin);
        T obj =(T) serializer.ReadObject(stream);
        return obj;
    }
    }

