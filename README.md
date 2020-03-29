# Wox.Plugin.Gen —— 为 Wox 提供生成 GUID(UUID)、随机数、Unix 时间戳、Base64 加解密的能力

- [Wox](https://github.com/Wox-launcher/Wox)
- [中文说明](./README.zh-cn.md)

## Usage

![sample](./resources/sample.gif)

Default trigger keyword `gen`

## Commands

### Auto complete
Enter the first few characters of the command, matching commands will be shown automatically, use <kbd>Tab</kbd> to switch between options and <kbd>Enter</kbd> to choose. This will become handy when the coomand is long.

### guid
```
guid [u]
```

Generate a GUID, by default the result will be in lower case with 4 styles.
- `00000000000000000000000000000000`
- `00000000-0000-0000-0000-000000000000`
- `{00000000-0000-0000-0000-000000000000}`
- `{0x00000000，0x0000，0x0000，{0x00，0x00，0x00，0x00，0x00，0x00，0x00，0x00}}`

Optional parameter
- `u` for generate GUID in all caps

### uuid
```
uuid [u]
```

Refer to `guid`

### rand
```
rand [[max]|[min max]]
```

Generates a random integer, by default between 0 (inclusive) and 100 (exclusive)

Optional parameter
- `max` Specifed maximum value, the upper bound of the range, exclusive.
- `min max` Specified min and max value, the number generated will be in range [min, max).

### roll
```
roll [[max]|[min max]]
```

Refer to `rand`

### unixtime
```
unixtime [unix_timestamp]
```

Conver the current time to Unix timestamp or covert the unix timestamp to date and time.

Optional parameter 
- `unix_timestamp` The Unix timestamp to be converted to date and time

### timestamp
```
timestamp [unix_timestamp]
```

Refer to `unixtime`

### encodebase64
```
encodebase64 text
```

Encode the content entered to Base64.

Required parameter
- `text` Plain text to encode

### decodebase64
```
decodebase64 base64
```

Decode the content enter from Base64

Required parameter
- `base64` Base64 content to decode

## Credits
- Localization
  - **English** [KevinSJ](https://github.com/KevinSJ)
- Basic icons from [Alibaba icon resources](https://www.iconfont.cn/)
