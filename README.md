# ProtobufTextSerializer

## 存在理由

protobuf-net本身不支持Protobuf官方的文本格式(简称PBT格式)

PBT格式比Json更简单, 且支持注释, 是一种良好的配置文件格式

本库支持PBT格式的序列化

## 特性支持

* 不依赖protobuf-net

* 文本输出支持单行/多行

* 序列化的标准仅限于public属性及List<>列表

* repeated字段只支持字段重复读取, 不支持<>标示及[]标示的数组队列

* 转义符只支持\r \n的常见转换

# 库依赖

https://github.com/davyxu/SharpLexer


# 链接

* protobuf-net运行库

	https://github.com/mgravell/protobuf-net