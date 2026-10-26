# 邓艾（普通移动版）

核对日期：2026-10-26。`mobile:deng-ai`，魏、男、4体力，神话再临·山；屯田82900，武将及当前池82999。

- [移动官网武将51](https://www.sanguosha.cn/pc/hero-detail-51.html)：普通屯田于回合外失去牌后可判定，生效后的非红桃判定牌成为“田”，出距离按田数减少；凿险准备阶段田不少于3时减1体力上限并获得急袭。不能套用界版红桃获得判定牌的收益。
- [移动BWIKI当前人物页](https://wiki.biligame.com/msgs/邓艾)，[固定修订34837](https://wiki.biligame.com/msgs/index.php?title=邓艾&oldid=34837)，标注2025-10-22更新。本文采用身份“经典”栏，魏男4、神话再临山包；不采用国战主将急袭或势邓艾。
- [屯田技能页](https://wiki.biligame.com/msgs/屯田)，[固定修订10302](https://wiki.biligame.com/msgs/index.php?title=屯田&oldid=10302)分别列普通非红桃成田与界版红桃得牌的区别。[凿险](https://wiki.biligame.com/msgs/index.php?title=凿险&oldid=10308)及[急袭](https://wiki.biligame.com/msgs/index.php?title=急袭&oldid=4006)普通、界版身份正文相同；未见测试、爆料标记。
- 官方经典画像：[5100.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/5100.jpg)，`portraitKey: mobile-deng-ai`，图鉴 `myth-mountain`。

## 实现与复用边界

屯田独立定义为 `mobile:tuntian`。现有 `classic:tuntian` 的手牌移牌入口明确排除原生使用、打出、无懈的付款原因，不能完整覆盖移动版回合外使用/打出牌；因此不按同名正文直接引用。普通移动版采用界移动屯田已有的 `cardsMoved` / `perOwnerBatch` 原生批次与手牌、装备来源，真正从本人手牌/装备拥有域离开才提供一次可选发动，同一批同时失去手牌和装备不重复。手牌与装备之间的内部移动不算失牌；仅失去判定区牌不算失去自己拥有的牌。

判定、改判、冻结生效结果及实体处理使用现有 `startJudgment` / `outcomeBind` 原生帧。红桃判定实体由处理区进入弃牌堆，不获得到手牌、不成为田；非红桃生效实体进入本人 `Authority` 持久区，展示名为“田”。非红桃包含无花色，不以“是黑色”或某一颜色代替。出距离减少复用 `negatedOwnedZoneCount`，反向距离不变。上述真实响应付款与判定、移牌的孩子均回到原帧，不创建平行待决状态。

凿险直接引用 `boundary:zaoxian-mobile`，该技能按实际本人准备阶段检查泛用 `Authority` 区，不绑定界屯田ID；减1体力上限及授技走现有共享技能变更层。其真实来源授予 `boundary:deng-ai-jixi-mobile`，该急袭将本人一张 `Authority` 实体作为原生顺手牵羊材料，没有额外花色限制，支付、目标、距离及后续锦囊均由原生流程校验。

没有引用 `classic:zaoxian`，因为其固定授予的 `classic:jixi` 把材料限定为黑桃、梅花、方片；“一张田”正文没有这个额外限制。沿用完整等价的既有界移动凿险/急袭，既不复制定义，也不修改旧版默认行为或历史内容哈希。
