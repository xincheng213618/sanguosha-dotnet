# 魏延·移动版

核查日期：2026-10-26。普通移动身份，蜀、男、4体力，神话再临风包；完整引用现有 `classic:kuanggu`，没有复制技能或运行时。

来源：[移动官网27](https://www.sanguosha.cn/pc/hero-detail-27.html)、[移动BWIKI人物固定版本34841](https://wiki.biligame.com/msgs/index.php?title=魏延&oldid=34841)（2025-10-21）、[普通狂骨固定版本20548](https://wiki.biligame.com/msgs/index.php?title=狂骨/魏延&oldid=20548)（2023-01-03）。人物普通经典栏列出44级获得，属于实际已上线的普通身份版本。

当前普通狂骨按每1点伤害分别触发，取受伤角色扣减体力前的距离；距离不大于1时，伤害来源回复1点体力。官网与移动技能页一致。[2024-11-25官方技能描述规范公告](https://www.sanguosha.cn/pc/news-detail-1959.html) 将普通狂骨列为规范描述调整对象，并明确本次调整不改变实际效果。

已有 `_shared/Skills/passive-damage-cutover.rules.json` 的 `classic:kuanggu` 使用 `afterDamageApplied`、`subject:damageSource`、`damageOccurrence:perDamagePoint`，按 `sourceToTargetDistanceAtDamage<=1` 执行本人原生回复。`GameEngine` 在实际扣血之前捕获距离，再由原生伤害上下文传给触发条件；不在回复子窗重新计算可能已经变化的距离。多点伤害按实际伤害点处理，满血时沿用现有原生回复资格，不增加额外摸牌收益。

本次不引用界魏延的回复/摸牌选择和奇谋，也不混用势魏延的每次伤害事件收益或使命升级。纯内容引用由共用加载、注册和伤害/回复机制检查覆盖；可沿用 `ShanheArmorChecks.FullAndPartialArmorKeepDamageCounterAndRealHealthDeltaCold` 对实际狂骨伤害点、体力变化及子返回的既有检查，不新增重复人物夹具。

经典立绘来自[官网2700.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/2700.jpg)，对应当前官网27皮肤列表经典首图；`portraitKey` 为 `mobile-wei-yan`，本地目标名 `official-mobile-wei-yan.jpg`，图鉴分组 `myth-wind`。
