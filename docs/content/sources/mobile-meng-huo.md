# 孟获·移动版

核查日期：2026-10-26。普通移动身份，蜀、男、4体力、神话再临林包。祸首引用 `classic:huoshou`；再起用独立 `mobile:zaiqi` 重排已有DSL操作，不新增运行时或opcode。

来源：[移动官网47](https://www.sanguosha.cn/pc/hero-detail-47.html)、[移动BWIKI人物35245](https://wiki.biligame.com/msgs/index.php?title=孟获&oldid=35245)、[祸首20604](https://wiki.biligame.com/msgs/index.php?title=祸首/孟获&oldid=20604)、[再起20605](https://wiki.biligame.com/msgs/index.php?title=再起/孟获&oldid=20605)。已通过 MediaWiki revisions API 读取原始wikitext：人物2025-12-19、两技能2023-01-03。经典栏为林包、开服上线、85级/招募/120将魂获取；当前官网与移动技能正文一致，不混用界或谋版。

祸首沿现有原生 `excludeGlobalTarget` 排除本人受南蛮效果，`attributeGlobalDamage` 令南蛮伤害归属于存活的技能拥有者。原生Group仍保存原使用者和全部物理材料；改变伤害来源不会把孟获伪装成牌使用者，也不会把巨象的“其他角色使用”误判成“其他角色造成伤害”。本人使用南蛮时本就是该伤害来源，没有额外改变材料归属。

普通再起在真实摸牌阶段开始、已受伤时可选择替代本次正常摸牌。展示数量是当前已损失体力X，不是X+1、不是至多3，也不是本回合弃过的牌数。公开展示真实牌堆顶实体，按其中红桃张数进行原生回复，然后处理红桃弃置及剩余实体获得。已发行的展示绑定和回复/获得子窗继续留在原再起帧。

现有 `classic:zaiqi` 的实际顺序是展示、分离红桃、弃红桃、先获得其余牌、最后回复；这与当前普通移动正文的先回复再获得有实际事件顺序差异，不能纯引用。移动JSON仅把既有 `recover` 前移到红桃和其他牌的后续处理之前：`revealTopCards(ownerLostHp)` → `filterBoundCards(heart)` → `recover(boundCardCount)` → 红桃进Discard → 其余牌进OwnerHand。每个动作仍走现有原生实体/回复/typed返回，没有手工改HP或手牌数。现有山河S5再起使用相同的先回复机制，但数量是X+1，不能把该完整技能当普通版引用。

本次代表性检查在真实混合花色展示后的回复子窗冷恢复，确认回复发生时非红桃尚在Processing、本人尚未获得；随后原实体各移动一次，正常摸牌被替代。纯祸首路径和共享展示/回复能力复用既有检查；统一构建与定向验证由主代理执行。

普通经典图：[4700.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/4700.jpg)，官网47第二张，首图44400为界版。`portraitKey` 为 `mobile-meng-huo`，图鉴分组 `myth-forest`，技能86400、人物/将池86499。
