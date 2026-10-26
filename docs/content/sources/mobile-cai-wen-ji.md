# 蔡文姬·移动版

核查日期：2026-10-26。普通移动身份，群、女、3体力，神话再临山包；新增 `mobile:beige`，完整引用 `classic:duanchang`。

来源：[移动官网57](https://www.sanguosha.cn/pc/hero-detail-57.html)、[移动BWIKI人物固定版本35286](https://wiki.biligame.com/msgs/index.php?title=蔡文姬&oldid=35286)（2025-12-20）、[普通悲歌固定版本38857](https://wiki.biligame.com/msgs/index.php?title=悲歌/蔡文姬&oldid=38857)（2026-06-07）、[普通断肠固定版本20770](https://wiki.biligame.com/msgs/index.php?title=断肠/蔡文姬&oldid=20770)。普通经典栏列出每日特惠或40000闲豆的实际获取方式；本次不使用国战的叠置与单张武将牌失技规则。

普通悲歌在一名角色受到杀伤害后，可先弃自己一张牌，再令受伤角色判定；红桃回复固定1，方块摸2，梅花由伤害来源弃2张，黑桃由伤害来源翻面。成本可使用手牌或装备，不限手牌。已有 `classic:beige` 的真实成本仅接受手牌，且发动条件要求手牌数大于0，因此不能直接引用；界移动悲歌的红桃回复本次伤害量、方块摸3也不等价。

移动定义复用已注册原语，在原 `afterDamageApplied` 帧上支付真实HE成本并等待移牌子结算，保存原受伤目标与伤害来源，再发起原生判定。四花色通过同一原帧的 `outcomeBind` 续接，沿用现有原生回复、摸牌、选弃与翻面；最后处理自己的真实判定牌，不增加人物运行时、操作码或平行pending。空手且有装备的合法发动资格由同一HE支付原语确定，不保留经典的手牌数量前提。

普通断肠与 `classic:duanchang` 的死亡触发、伤害来源身份及失去全部当前武将技能规则相同，直接复用既有 `loseDeathSourceSkills` 与共享技能变化合同，不复制定义。普通移动人物按现行来源设为Female；未修改其他版本的资料。

focused检查只覆盖此前缺失的空手装备费用、真实杀伤害、原生判定子返回与一次cold，复用成熟四花色和技能失去机制，不重复完整悲歌/断肠矩阵。

经典立绘来自[官网5700.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/5700.jpg)，`portraitKey` 为 `mobile-cai-wen-ji`，本地目标名 `official-mobile-cai-wen-ji.jpg`，图鉴分组 `myth-mountain`。
