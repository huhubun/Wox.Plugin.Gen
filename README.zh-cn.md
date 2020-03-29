# Wox.Plugin.Gen —— 为 Wox 提供生成 GUID(UUID)、随机数、Unix 时间戳、Base64 加解密的能力

- [Wox](https://github.com/Wox-launcher/Wox)
- [English documentation](./README.md)

## 使用

![sample](./resources/sample.gif)

默认快捷键 `gen`

## 指令

### 自动补全
输入指令的开头几个字母，会自动显示出匹配的指令，通过 <kbd>Tab</kbd> 来切换，通过 <kbd>Enter</kbd> 来选中。当指令名称很长时，能提高输入的效率。

### guid
```
guid [u]
```

生成一个 GUID，默认为小写、四种样式的 GUID：
- `00000000000000000000000000000000`
- `00000000-0000-0000-0000-000000000000`
- `{00000000-0000-0000-0000-000000000000}`
- `{0x00000000，0x0000，0x0000，{0x00，0x00，0x00，0x00，0x00，0x00，0x00，0x00}}`

可选参数
- `u` 生成大写 GUID

### uuid
```
uuid [u]
```

同 `guid`

### rand
```
rand [[max]|[min max]]
```

生成一个随机整数，默认为 [0, 100) 间

可选参数
- `max` 指定一个最大值，将生成 [0, max) 间的随机数
- `min max` 指定最小值和最大值，将生成 [min, max) 间的随机数

### roll
```
roll [[max]|[min max]]
```

同 `rand`

### unixtime
```
unixtime [unix_timestamp]
```

生成当前时间的 Unix 时间戳，或将填写的时间戳转为日期和时间

可选参数
- `unix_timestamp` 指定要转换的 Unix 时间戳

### timestamp
```
timestamp [unix_timestamp]
```

同 `unixtime`

### encodebase64
```
encodebase64 text
```

为输入的内容进行 Base64 编码

必填参数
- `text` 要进行 Base64 编码的字符串

### decodebase64
```
decodebase64 base64
```

为输入的内容进行 Base64 解码

必填参数
- `base64` 要进行解码的 Base64 字符串

## 鸣谢
- 本地化
  - **English** [KevinSJ](https://github.com/KevinSJ)
- 基础图标资源（不含背景色）来自 [阿里巴巴矢量图标库](https://www.iconfont.cn/)