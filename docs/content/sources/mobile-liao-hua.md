# 移动版廖化

> 此版本与 `classic:liao-hua` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核对日期：2026-10-25。正式身份 `mobile:liao-hua`，蜀、男性、四体力，版本 `mobile`，规则集 `sanguosha-mobile`，画像键 `mobile-liao-hua`。

- [移动版官网廖化115](https://www.sanguosha.cn/index.php/pc/hero-detail-115.html)：完整HTML同时含默认显示的当先及默认隐藏的伏枥。当先在回合开始执行额外出牌阶段；伏枥为自身濒死时的限定回复，回复至全场现存势力数，然后翻面。
- [移动版Wiki廖化](https://wiki.biligame.com/msgs/廖化)，2026-01-05更新：现行身份的两项能力与官网一致。检索移动版官方后续调整公告，未发现替代普通身份规则的公告。

完整复用 `classic:dangxian` 与 `classic:fuli`。当先的原生插入阶段发生在正常准备阶段之前；不从弃牌堆取杀。伏枥通过原生濒死回应，本局限一次，体力回复至存活角色的势力数并受当前体力上限限制，随后真实翻面。翻面不要求成为全场唯一最高体力，也不是强制设置为背面；已背面时会翻正。该规则与移动版界廖化的取杀和“回复后唯一最高才翻面”不同。

本次为完整能力的纯配置复用，不新增人物专用测试、Runtime、枚举或共享接线。额外阶段、限定次数、濒死回复及孩子结算恢复沿既有原生生命周期实现维护。

独立画像为官网首张[经典形象51400](https://www.sanguosha.cn/storage/uploads/images/skins/51400.jpg)，原始530×750 JPEG，未裁剪或改色，本地 `src/CardGame.Wpf/Assets/official-boundary-liao-hua-mobile.jpg`。
