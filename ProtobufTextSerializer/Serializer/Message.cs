﻿
using System;
namespace ProtobufTextSerializer
{
    public class Message
    {
        object _ins;
        Type _type;
        public Message(object ins)
        {
            _ins = ins;
            _type = ins.GetType();
        }


        public void SetValue( string field, string value )
        {
            var prop = _type.GetProperty(field);
            if (prop == null)
                return;

            object v = Convert.ChangeType(value, prop.PropertyType);

            prop.SetValue(_ins, v, null);
        }

        public Message AddMessage(string field)
        {
            var prop = _type.GetProperty(field);
            if (prop == null)
                return null;

            // 泛型肯定是List, 且肯定是Repeated
            if ( prop.PropertyType.IsGenericType )
            {
                // 取泛型第一个类型
                var elementType = prop.PropertyType.GetGenericArguments()[0];

                // 创建List<T>中的T类型实例
                var newIns = Activator.CreateInstance(elementType);

                // 找到这个字段List的实例
                var listIns = prop.GetValue(_ins, null);

                // 取出List的Add函数
                var listAdd = listIns.GetType().GetMethod("Add");

                // 调用Add函数传入创建好的对象
                listAdd.Invoke(listIns, new object[]{newIns});


                var msg = new Message(newIns);


                return msg;
            }
            else
            {
                var newIns = Activator.CreateInstance(prop.PropertyType);

                var msg = new Message(newIns);


                prop.SetValue(_ins, newIns, null);

                return msg;
            }

           
        }

        



    }
}
