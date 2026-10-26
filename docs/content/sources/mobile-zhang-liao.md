# 张辽（普通移动版）

核对日期：2026-10-26。`mobile:zhang-liao`，魏、男、4体力，标准包；突袭84800，武将及当前池84899。

- [移动官网武将18](https://www.sanguosha.cn/pc/hero-detail-18.html)：摸牌阶段可改为获得至多两名角色各一张手牌。当前移动[人物页](https://wiki.biligame.com/msgs/张辽)、[固定修订34816](https://wiki.biligame.com/msgs/index.php?title=张辽&oldid=34816)的身份经典栏明确为“其他角色”；魏男4、开服标准包、15级获取。
- [普通突袭当前页](https://wiki.biligame.com/msgs/突袭/张辽)、[固定修订20080](https://wiki.biligame.com/msgs/index.php?title=突袭/张辽&oldid=20080)，标注2022-11-30更新，与现行人物正文相同。采用身份经典，未采用1V1手牌数少于对手才少摸1的版本，也未采用界版按少摸数量选择角色。
- 官方经典画像：[1800.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/1800.jpg)，`portraitKey: mobile-zhang-liao`，图鉴 `standard`。官网首张31400是界版，普通图为1800。

## 实现与复用边界

独立定义 `mobile:tuxi`，使用原生 `drawPhaseStarting` 的 `replacement` 入口；“至多两名”包含0名，不能用不发动而正常摸牌代替。即使其他人都没有手牌，仍可发动替代并选择空集合。目标只能是存活、有手牌的其他角色，不获得自己手牌。

旧 `classic:tuxi` 定义为最少1名、最多2名，不能完整引用。现有 `selectTargets` 的加载、目标resolver、组合资源和实际选择校验已经允许该目标类型的0至2名集合。取得牌复用 `takeRandomHandCardFromSelectedTargets` 的 `awaitMovementTriggers:true` 路径：该资源合同明确读取0至2名集合且只能消费一次，原生原帧回执也明确允许零张实体。没有放宽图校验、伪造一个目标或新增人物待决状态。

发动后选0名仍结清本次摸牌替代，不执行正常摸牌、不获得任何实体；空回执仅记录实际取得0张，不构造得牌或摸牌事实。选择1至2名时继续原生实体取得及移牌子窗，取得不算摸牌；这部分已有突袭/替代摸牌的共享覆盖，本人物只补零目标、无人有手牌时仍可发动及选择窗代表冷恢复的定向行为。
