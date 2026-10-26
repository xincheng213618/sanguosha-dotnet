# 张让·移动版

核对日期：2026-10-26。普通移动身份 `mobile:zhang-rang`，群、男、3体力，乱世英杰2016（图鉴 `fame-6`），经典画师蚂蚁君。

来源：[移动官网224](https://www.sanguosha.cn/pc/hero-detail-224.html)、[移动BWIKI张让](https://wiki.biligame.com/msgs/张让)（[oldid35664](https://wiki.biligame.com/msgs/index.php?title=张让&oldid=35664)，2026-01-08更新）。BWIKI普通经典栏列出6000将魂合成或招募获得，完整技能属于已上线普通身份规则。

滔乱在无人濒死时，将一张牌当基本牌或普通锦囊牌使用，每种牌名每局限一次。该牌完整结算结束后，令其他一名角色选择交给张让一张与声明牌类别不同的牌，或使张让失去1点体力并令本回合滔乱失效。正式使用型响应沿用其原生Use身份，不把只打出一张牌改算成使用。

本库 `classic:taoluan` 已完整提供真实实体转换、牌名账本、无人濒死资格、手牌/装备/木牛流马材料、正式使用响应、整张牌结算完成后一次索牌与异类别赠牌。移动版继续复用这些能力，但拒绝分支的时序不能直接引用旧ID：官网明确写失去体力“然后”滔乱失效，当前BWIKI用“且”连接两项，没有明文要求将失效提前到体力流失前。旧经典JSON先设失效再执行失HP，移动版据官方明确顺序独立注册 `mobile:taoluan`。

移动规则只调整原生节点编排：`selectTarget → chooseDifferentActionCategoryGift → loseHp → setBooleanState`；两个尾节点仍仅在同一真实拒绝结果下执行。体力流失、其健康观察与濒死子窗均在原结算帧上完成，再返回末尾失效节点；费用、拒绝事实和牌名不会重新支付或记录。状态继续用既有公开布尔状态，按回合重置；不增加新opcode、并行pending、通用机制分支或格式版本，也不修改经典定义。

`MobileZhangRangChecks.DeclineLosesHpBeforeDisablingAndReturnsOriginalFrameCold` 对比经典和移动在真实失HP子窗中的状态，恢复后核对原帧只付一次、只失HP一次并设一次失效。`DeclinedLastHpEntersNativeDyingBeforeDisableAndDiesCold` 从真实最后1点体力进入原生濒死，在失效尚未执行时冷恢复，继续真实放弃救援及死亡；不强行让死亡角色执行剩余尾节点。既有 `Fame2016TaoluanChecks.EquipmentGlobalAndNullification` 继续负责完整装备材料、群体牌与正式无懈响应的共享机制，不复制历史夹具矩阵。

经典立绘采用[官网38700.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/38700.jpg)，portraitKey `mobile-zhang-rang`。该链接已从官网当前HTML首图核对；官网另列38701皮肤，不替代经典原画。构建与实际检查结果由批次统一验证记录。
