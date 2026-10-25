# 移动版界黄月英

核对日期：2026-10-25。独立人物 `boundary:huang-yueying-mobile`，女性、蜀势力、3体力；画像键 `boundary-huang-yueying-mobile`，规则集 `sanguosha-mobile`。

- [移动版官网武将详情233](https://www.sanguosha.cn/index.php/pc/hero-detail-233.html)：集智正文为使用普通锦囊后可摸一张，若本次取得基本牌，可弃置该牌换取本回合手牌上限加一。它没有非转化牌限制，也不是对延时锦囊发动。官网玩法同时提到锦囊距离和防具保护。
- [移动版Wiki界黄月英](https://wiki.biligame.com/msgs/%E7%95%8C%E9%BB%84%E6%9C%88%E8%8B%B1)，2025-10-26更新：身份版集智与官网一致；身份奇才明确锦囊无距离限制，且其他角色不能弃置装备区防具、宝物。页面另列1V1只保护防具的条目，不用于本身份人物。
- [移动版Wiki奇才条目](https://wiki.biligame.com/msgs/%E5%A5%87%E6%89%8D/%E7%95%8C%E9%BB%84%E6%9C%88%E8%8B%B1)：交叉核对完整身份保护范围。

现有OL界版 `boundary:jizhi-current` 是非转化锦囊，包括延时锦囊；本移动版独立登记 `boundary:jizhi-mobile`，按普通锦囊类别匹配，并允许合法转化、虚拟普通锦囊。锦囊的真实使用与原生无懈可击响应使用分别沿现有 `cardUseTargetsFinalized`、`cardResponseAccepted` 两个窗口，后者只接无懈可击，不将普通打出事件当成锦囊使用。

摸牌、获取本次牌的绑定、保留或弃置选项、实际弃置和回合手牌上限修正完整使用现有通用节点。只能弃置本次真正取得、仍合法持有的基本牌，不能拿原手牌替换；摸牌子结算、选项与原使用帧及恢复继续沿既有合同，没有新Runtime、pending、事件、枚举或版本号。

奇才与现有 `boundary:qicai-current` 完全一致，直接复用该技能。保护范围是装备区防具和宝物，并非武器；他人获得、本人支付或装备替换不因此自动被取消。锦囊距离仍由现有原生策略处理。

新增最少行为回归覆盖本版此前缺少的合法转化普通锦囊正向集智及真正延时锦囊排除，并在本次摸得基本牌的处置边界冷恢复。既有 `Unconverted Trick actual use qualification` 继续覆盖OL界版的相反转化限制；不复制奇才的整套共同机制夹具。

新增检查入口为 `BoundaryHuangYueyingMobileChecks.ConvertedOrdinaryTrickPaysDrawnBasicForLimitColdAndDelayedTrickDoesNotTrigger`。它使用既有资格检查已验证的四人发牌布局，不搜索种子；生产规则复用既有摸牌、牌绑定、弃置和手牌上限节点。
