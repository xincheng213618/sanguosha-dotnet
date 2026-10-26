# 孙权（普通移动版）

核对日期：2026-10-26。`mobile:sun-quan`，吴、男、4体力，标准包；救援84400，武将及当前池84499。

- [移动官网武将8](https://www.sanguosha.cn/pc/hero-detail-8.html)原始HTML救援隐藏标签正文明确：主公技、锁定技，其他吴势力角色对你使用桃，回复体力加1，没有本人濒死限定。
- [移动BWIKI当前人物](https://wiki.biligame.com/msgs/孙权)身份经典栏同样写其他吴角色用桃的该牌回复值加1；与国战至多体力上限张、1V1至多2张的制衡区分。现行主文与官网一致，没有测试或爆料标记。
- 本批前置研究已记录[人物修订36276](https://wiki.biligame.com/msgs/index.php?title=孙权&oldid=36276)、[制衡20158](https://wiki.biligame.com/msgs/index.php?title=制衡/孙权&oldid=20158)、[救援26602](https://wiki.biligame.com/msgs/index.php?title=救援/孙权&oldid=26602)。本次重新获取当前人物正文及官网原始隐藏正文；固定修订入口本次读取失败，不将缓存页面的更新时间当作本次复核到的修订时间。
- 官方经典画像：[800.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/800.jpg)，`portraitKey: mobile-sun-quan`，图鉴 `standard`；40200为界版。

## 实现与复用边界

制衡完整引用 `classic:zhiheng`：真实出牌阶段限一次，手牌、装备任意正数张，弃置后摸等量，不附带弃全部手牌多摸1的界版收益。

救援独立定义为 `mobile:jiuyuan`。旧 `classic:jiuyuan` 文字及原生入口把额外回复限定为濒死救桃；普通移动正文没有这一限制。原生红色牌扩目标机制已经允许吴角色把自己的桃额外指定给非濒死孙权，不能以通常只能自用桃为由忽略这个真实合法入口。

新增明确可选 `peachRecoveryScope: nativePeach`，只允许桃、无条件、主公资格及来源势力明确的救援策略。省略字段仍沿旧范围，序列化省略null，不改变经典定义或其内容指纹。实际原生桃给各目标回复时，校验原 `CardUseFrame`、行动nonce、真实使用者及被指定目标，再复用现有技能来源、主公资格与实际使用者吴势力条件查询；只给其他角色使用的桃加值。普通技能回复、酒及其他回复不会进入该范围。

濒死原生入口已签发的策略来源继续保留，末端按技能与策略ID去重，不再加第二次。收益保留在原生桃的回复参数及策略来源回执中，原回复、HP子窗和用牌实体继续同一帧，不新增人物待决状态。

旧范围缺口由 `ResolvePeach` 仅在allowDying读取策略、扩目标各recipient沿用原continuation基数的代码路径静态定位；本批未另行构建执行旧版失败见证，不能记作已执行的旧版FAIL。新增定向行为使用正式移动内容与同一真实红桃扩目标流程，另核经典null范围仍回复1、普通非桃技能回复仍为1，并保留已生效HP子窗的代表冷恢复；实际结果由统一验证记录提供。
