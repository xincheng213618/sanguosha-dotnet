# 夏侯霸·移动版

核对日期：2026-10-26。独立身份 `mobile:xiahou-ba`，蜀势力，男性，4体力，SP武将组1。经典形象画师熊猫探员。

规则来源：[移动官网78](https://www.sanguosha.cn/pc/hero-detail-78.html)、[移动版BWIKI夏侯霸](https://wiki.biligame.com/msgs/夏侯霸)（当前页面 oldid=35875，2026-01-11 更新）。BWIKI经典身份栏注明300将魂合成或招募获得，另将势夏侯霸列为独立歧义入口。本实现采用已公开普通身份规则，不混用势版本、国战豹烈、1V1或十周年永久获得技能的版本。

豹变完整复用 `ol:baobian`：当前体力不大于3时视为拥有挑衅，不大于2时还视为拥有咆哮，为1时再视为拥有神速。资格按实际当前体力动态派生，体力回复后重新判断，不把曾经满足过阈值当作永久获得。

派生技能沿用该现有定义精确引用的 `classic:tiaoxin`、`classic:paoxiao` 和 `classic:shensu`。挑衅每个出牌阶段一次，选择攻击范围内包含自己的其他角色，要求其对自己使用杀，否则弃其一牌；咆哮为使用杀无次数限制；神速保留跳过判定和摸牌、跳过出牌并弃装备这两项，每项使用一张无距离限制的普通杀。与当前普通移动版正文一致，因此仅新增人物身份资料，不复制技能JSON或新建重复人物测试。

现有 `HP conditional grants: actual HP thresholds phase quota and cold replay` 检查已覆盖真实伤害、体力流失、回复导致的动态授予与撤回，以及原技能实例、阶段次数和冷恢复。新身份继续复用该共享机制与内容注册校验。

经典立绘采用[移动官网11600](https://www.sanguosha.cn/storage/uploads/images/skins/11600.jpg)，资源键 `mobile-xiahou-ba`；图鉴归入SP组。
