# 武将按实际技能去重

本项目尚未发布。本次直接删除重复武将定义、池注册和重复图鉴入口，不保留被删除编号的加载或存档兼容，也不重写存档指纹。神将中之前已隐藏的等价定义一并移除。

核对使用完整技能列表与实际执行规则，不以姓名、平台名、技能描述或立绘相同作为充分条件。同技能 ID 的组还核对了势力、体力、初始体力、性别及原生代码是否按武将编号分流。国战和活动专用角色有独立的模式身份，不并入普通身份武将。

技能的次数、数值、触发时机、代价、作用范围、响应资格或结算顺序不同，仍保留独立版本。例如孙策的主公技、庞统的连环与涅槃、荀彧的驱虎与节命、刘禅的享乐与若愚均有实际区别。陆绩、阚泽、诸葛恪、界马超、界孙权、神许褚和神张飞还存在原生执行中的差异，不能仅凭近似规则 JSON 合并。神将的逐组差异见 [神将规则核对](GOD_GENERAL_RULES.md)。

`GeneralRulesCatalog` 的映射仅用于图鉴素材归属与去重检查，不是游戏命令或存档读取的重定向。被删编号不再注册。独立立绘归入保留武将的皮肤，素材来源继续保留；没有增加新的兼容层。

## 删除名单

本次移除 87 个重复可选版本，并删除 7 个先前已隐藏的神将等价定义，共 94 个武将定义。同时删除 13 张逐字节 SHA-256 相同的重复图片文件，素材引用指向现有相同图片。

| 删除编号 | 保留编号 | 武将 | 核对依据 |
| --- | --- | --- | --- |
| `anniversary:shen-dian-wei` | `ol:shen-dian-wei` | 神典韦 | 既有神将逐组规则核对 |
| `boundary:da-qiao-mobile` | `boundary:da-qiao` | 界大乔 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `boundary:gan-ning-mobile` | `boundary:gan-ning` | 界甘宁 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `boundary:guan-yu-mobile` | `boundary:guan-yu` | 界关羽 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `boundary:guo-jia-mobile` | `boundary:guo-jia` | 界郭嘉 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `boundary:huang-gai-mobile` | `boundary:huang-gai` | 界黄盖 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `boundary:lu-bu-mobile` | `boundary:lu-bu` | 界吕布 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `boundary:lu-xun-mobile` | `boundary:lu-xun` | 界陆逊 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `boundary:sima-yi-mobile` | `boundary:sima-yi` | 界司马懿 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `boundary:sun-shangxiang-mobile` | `boundary:sun-shangxiang` | 界孙尚香 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `boundary:xu-sheng-mobile` | `boundary:xu-sheng` | 界徐盛 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `boundary:zhen-ji-mobile` | `boundary:zhen-ji` | 界甄姬 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `boundary:zhou-yu-mobile` | `boundary:zhou-yu` | 界周瑜 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `boundary:zhuge-liang-mobile` | `boundary:zhuge-liang` | 界诸葛亮 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:bei-mi-hu` | `ol:bei-mi-hu` | 卑弥呼·OL | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:bu-zhi` | `ol:bu-zhi` | 步骘·OL | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:cao-cao` | `classic:cao-cao` | 曹操 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:cao-chong` | `classic:cao-chong` | 曹冲 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:cao-hong` | `ol:cao-hong` | 曹洪 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:cao-rui` | `classic:cao-rui` | 曹叡 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:cao-xiu` | `classic:cao-xiu` | 曹休 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:cao-zhang` | `classic:cao-zhang` | 曹彰 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:cao-zhi` | `classic:cao-zhi` | 曹植 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:chen-gong` | `classic:chen-gong` | 陈宫 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:chen-lin` | `ol:chen-lin` | 陈琳 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:dong-zhuo` | `classic:dong-zhuo` | 董卓 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:fu-wan` | `ol:fu-wan` | 伏完 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:gan-ning` | `classic:gan-ning` | 甘宁 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:guan-ping` | `classic:guan-ping` | 关平 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:guan-yu` | `classic:guan-yu` | 关羽 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:guo-tu-feng-ji` | `classic:guo-tu-feng-ji` | 郭图逢纪 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:han-dang` | `classic:han-dang` | 韩当 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:hao-zhao` | `classic:hao-zhao` | 郝昭 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:hua-tuo` | `classic:hua-tuo` | 华佗 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:huang-gai` | `classic:huang-gai` | 黄盖 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:huang-hao` | `classic:huang-hao` | 黄皓 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:huang-yueying` | `classic:huang-yueying` | 黄月英 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:jia-xu` | `classic:jia-xu` | 贾诩 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:jian-yong` | `classic:jian-yong` | 简雍 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:jiang-wei` | `classic:jiang-wei` | 姜维 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:li-tong` | `ol:li-tong` | 李通 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:liao-hua` | `classic:liao-hua` | 廖化 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:ling-tong` | `classic:ling-tong` | 凌统 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:liu-biao` | `classic:liu-biao` | 刘表 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:liu-chen` | `classic:liu-chen` | 刘谌 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:liu-feng` | `classic:liu-feng` | 刘封 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:liu-qi` | `ol:liu-qi` | 刘琦·OL | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:liu-yu` | `classic:liu-yu` | 刘虞 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:lu-bu` | `classic:lu-bu` | 吕布 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:lu-xun` | `classic:lu-xun` | 陆逊 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:man-chong` | `classic:man-chong` | 满宠 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:mi-zhu` | `ol:mi-zhu` | 糜竺 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:pan-zhang-ma-zhong` | `classic:pan-zhang-ma-zhong` | 潘璋马忠 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:pang-de` | `classic:pang-de` | 庞德 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:qin-mi` | `classic:qin-mi` | 秦宓 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:quan-cong` | `classic:quan-cong` | 全琮 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:sha-mo-ke` | `classic:sha-mo-ke` | 沙摩柯 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:shen-guan-yu` | `classic:shen-guan-yu` | 神关羽 | 既有神将逐组规则核对 |
| `mobile:shen-lu-meng` | `classic:shen-lu-meng` | 神吕蒙 | 既有神将逐组规则核对 |
| `mobile:shen-zhou-yu` | `classic:shen-zhou-yu` | 神周瑜 | 既有神将逐组规则核对 |
| `mobile:sima-lang` | `ol:sima-lang` | 司马朗 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:sun-deng` | `classic:sun-deng` | 孙登 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:sun-jian` | `classic:sun-jian` | 孙坚 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:sun-lu-ban` | `classic:sun-lu-ban` | 孙鲁班 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:sun-qian` | `ol:sun-qian` | 孙乾 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:wang-ji` | `classic:wang-ji` | 王基 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:wang-ping` | `classic:wang-ping` | 王平 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:wei-yan` | `classic:wei-yan` | 魏延 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:wen-pin` | `ol:wen-pin` | 文聘·OL | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:wu-guo-tai` | `classic:wu-guo-tai` | 吴国太 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:xi-zhi-cai` | `ol:xi-zhi-cai` | 戏志才·OL | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:xiahou-ba` | `ol:xiahou-ba` | 夏侯霸·OL | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:xiahou-dun` | `classic:xiahou-dun` | 夏侯惇 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:xiahou-yuan` | `classic:xiahou-yuan` | 夏侯渊 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:xu-huang` | `classic:xu-huang` | 徐晃 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:xu-shi` | `classic:xu-shi` | 徐氏 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:xun-you` | `classic:xun-you` | 荀攸 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:yan-yan` | `classic:yan-yan` | 严颜 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:yu-fan` | `classic:yu-fan` | 虞翻 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:yue-jin` | `ol:yue-jin` | 乐进 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:zhang-fei` | `classic:zhang-fei` | 张飞 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:zhang-lu` | `ol:zhang-lu` | 张鲁·OL | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:zhang-xiu` | `classic:zhang-xiu` | 张绣 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:zhao-yun` | `classic:zhao-yun` | 赵云 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:zhu-ran` | `classic:zhu-ran` | 朱然 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:zhu-rong` | `classic:zhu-rong` | 祝融 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:zhu-zhi` | `classic:zhu-zhi` | 朱治 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:zhuge-dan` | `classic:zhuge-dan` | 诸葛诞·经典 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:zhuge-liang` | `classic:zhuge-liang` | 诸葛亮 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:zhuge-zhan` | `classic:zhuge-zhan` | 诸葛瞻 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `mobile:zu-mao` | `ol:zu-mao` | 祖茂 | 完整技能 ID 与基础属性一致，原生无编号分流 |
| `ol:shen-lu-bu` | `classic:shen-lu-bu` | 神吕布 | 既有神将逐组规则核对 |
| `ol:shen-zhou-yu` | `classic:shen-zhou-yu` | 神周瑜 | 既有神将逐组规则核对 |
| `ol:shen-zhuge-liang` | `mobile:shen-zhuge-liang` | 神诸葛亮 | 既有神将逐组规则核对 |
