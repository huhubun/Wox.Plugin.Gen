# Wox.Plugin.Gen —— Wox 随机生成插件

- [Wox](https://github.com/Wox-launcher/Wox)

## 使用

![sample](./resources/sample.gif)

默认快捷键 `gen`

## 指令
### guid
生成一个 GUID，默认为小写

可选参数
- `u` 生成大写 GUID

### uuid
同 `guid`

### rand
生成一个随机整数，默认为 [0, 100) 间

可选参数
- `max` 指定一个最大值，将生成 [0, max) 间的随机数
- `min max` 指定最小值和最大值，将生成 [min, max) 间的随机数

### roll
同 `rand`

## TODO
- 多语言支持