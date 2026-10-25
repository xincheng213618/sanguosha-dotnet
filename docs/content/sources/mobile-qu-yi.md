# 麹义·移动版

核对日期：2026-10-26。独立 `mobile:qu-yi`，群、男性、4体力，SP武将组6。

[移动官网214](https://www.sanguosha.cn/pc/hero-detail-214.html)明确某其他角色到麹义距离为1时不能响应；[当前移动BWIKI身份正文（oldid40570）](https://wiki.biligame.com/msgs/index.php?title=麹义&oldid=40570)与[伏骑技能页（oldid38747）](https://wiki.biligame.com/msgs/index.php?title=伏骑&oldid=38747)把触发牌明确为杀和普通锦囊。采用这些正文共同确定的角色到技能拥有者方向，不借“与你距离”模糊文字改成反向。

骄恣完整引用 `classic:jiaozi` 的全场唯一最多手牌、造成或受到伤害加1规则。伏骑新增 `mobile:fuqi` 配置，在既有 `prohibitNearbyTargetResponse` 上明确 `useIncomingResponseDistance: true` 与 `includeNonTargetResponders: true`；不能只限制牌的直接目标而放行旁观者对原锦囊的无懈。省略两字段的经典定义继续采用原先拥有者到响应者方向与直接目标范围，旧定义序列化省略新增nullable字段。

杀、决斗、群体锦囊及无懈使用原生响应禁用点，独立定向检查覆盖非对称有向距离、非目标无懈持有者与一次真实响应窗口冷恢复。没有为移动身份改写经典骄恣或已有伤害结算。

经典画像采用官网普通经典图，精确URL与哈希见通用图库 `mobile-qu-yi`；本地为 `src/CardGame.Wpf/Assets/official-mobile-qu-yi.jpg`。
