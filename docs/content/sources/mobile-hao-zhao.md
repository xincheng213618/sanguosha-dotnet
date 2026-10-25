# 移动版郝昭

核对日期：2026-10-26。正式身份 `mobile:hao-zhao`，魏、男性、基础四体力，人物 `character:hao-zhao`，版本 `mobile`、规则集 `sanguosha-mobile`、画像键 `mobile-hao-zhao`；注册与当前将池顺序均为80399。移动Wiki归入神话再临雷包，图库使用 `myth-thunder`。

[移动官网345](https://www.sanguosha.cn/pc/hero-detail-345.html)与[移动Wiki当前郝昭](https://wiki.biligame.com/msgs/郝昭)（人物固定版本oldid31304，2025-06-29更新）的经典身份镇骨规则一致：自己的结束阶段可选择一名其他角色，在自己当前回合结束与该角色下个回合结束两个时点，分别按郝昭届时手牌数调整该角色手牌。补牌最多使其达到五张；弃牌仍应弃至郝昭手牌数，不能误作统一将目标值截成五张。人物页列有合成获取方式，属于正式普通移动身份；本次核对未发现已上线的移动重制与上述正文冲突。

完整复用已有 `classic:zhengu`，不重新注册同名技能。定义位于 `src/Characters/hao-zhao/classic/Skills/classic-hao-zhao.rules.json`，通过 `scheduleDeferredHandAlignment` 发行两个到期任务，再由 `deferredTurnEndOnly` 的 `resolveDeferredHandAlignment` 延期续体处理。现有 `GameEngine.DeferredHandAlignments.cs` 在真实到期时读取双方手牌，沿原生摸牌或弃牌及其子结算完成；没有新建移动身份专属回合状态，也不在发动时预先冻结两次目标手牌数。本项仅增加人物内容，沿用共享加载、延期任务与实体移动机制检查，无新运行时或重复人物定义快照测试。

正式经典画像为[官网46600](https://www.sanguosha.cn/storage/uploads/images/skins/46600.jpg)，由官网345原始HTML皮肤列表首图确认；后两图为46601、46602。经典图HTTP HEAD返回200、image/jpeg、246644字节。建议本地素材 `src/CardGame.Wpf/Assets/official-mobile-hao-zhao.jpg`，由批次统一素材与图库接入。
