# 张星彩（普通移动版）来源与实现

核对日期：2026-10-26。身份 `mobile:zhang-xing-cai`，人物 `character:zhang-xing-cai`，蜀、女、3体力，SP武将组5。技能注册顺序82200、82201，武将及当前将池顺序82299；肖像键 `mobile-zhang-xing-cai`。

## 当前正式资料

| 资料 | 本次核对内容 |
| --- | --- |
| [移动官网张星彩85](https://www.sanguosha.cn/pc/hero-detail-85.html) | 正式人物及两技能；甚贤没有每回合次数上限。枪舞仍保留“直到回合结束”的旧期限。 |
| [移动BWIKI张星彩](https://wiki.biligame.com/msgs/张星彩)（[固定修订34647](https://wiki.biligame.com/msgs/index.php?title=张星彩&oldid=34647)） | 普通身份的蜀女3、SP武将组5，已有活动兑换、5000将魂和招募获取渠道；正文明确枪舞“本阶段”。不使用国战或其他产品版本。 |
| [移动甚贤专页](https://wiki.biligame.com/msgs/甚贤)（[固定修订7527](https://wiki.biligame.com/msgs/index.php?title=甚贤&oldid=7527)） | 回合外，其他角色因弃置失去基本牌后，可摸1；没有每回合一次限制。 |
| [移动枪舞专页](https://wiki.biligame.com/msgs/枪舞)（[固定修订38933](https://wiki.biligame.com/msgs/index.php?title=枪舞&oldid=38933)） | 出牌阶段限一次，真实判定；本阶段小于判定点数的杀不限距离、大于判定点数的杀不计次数。 |

采用当前移动人物页与技能专页一致的**本阶段**期限，并明确记录官网旧正文冲突。不是为了复用而将OL“本回合”当成移动现行规则。等于判定点数的杀没有上述两项许可；不把“大于点数不计次数”实现成额外固定次数或用后返还。

经典立绘使用[官网12400.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/12400.jpg)，对应官网85经典首图。下载、素材目录与图鉴由批次集成维护。

## 与已有OL实现的差异

本库 `ol:shenxian` 明确每个实际回合限一次，`ol:qiangwu` 的许可按实际回合签发。因此两技能均使用独立移动定义，复用已有执行原语，不直接引用非等价的OL技能ID。

甚贤继续使用 `drawFromOtherActualBasicDiscard`。其原生观察器核对真实其他弃牌者、实际回合外、基本牌、逻辑弃置来源及原移动批次。专属定义合同只接受两种精确形状：原OL的 `usageScope:turn / usageLimit:1`，或移动版省略全部次数字段。每个真实弃置批次仍只能发一次收益，多个基本牌同批弃置不拆成多次摸牌；独立批次可在同一实际回合再次发动。每次收益先在原观察帧提交已签发事实，再由通用摸牌和其获得牌子窗返回，已支付子窗恢复不会重复发牌。

枪舞继续使用 `startJudgment → grantJudgedRankSplitSlashTurnPolicy`。后者新增可选 `expiry:"actualPlayPhase"`；省略保留原OL的实际回合期限。新标量领域枚举 `JudgedRankSlashPolicyExpiry.ActualPlayPhase=0` 位于既有专属合同文件中，没有新增操作码。移动许可在原判定父帧完成时冻结现有真实 `PhaseInstanceId`；候选用牌必须仍处于签发的真实出牌阶段。同回合插入的另一个出牌阶段不会继承，离开出牌阶段也没有新的候选许可。

最终实体判定、原技能来源及内容指纹、原父帧与指令游标沿用现有合同；无牌判定不签发伪造阈值。已提交的真实杀仍使用原 `JudgedRankSlashUseReceipt` 冻结点数、距离及次数决定，原生卡牌支付、伤害和完成事件不替换为技能伤害；在子窗恢复时不从当前手牌或当前技能状态猜测历史决定。

## 定向覆盖

`MobileZhangXingCaiChecks.UnlimitedSeparateActualBasicDiscardBatchesAndPaidDrawCold`：一个真实多基本牌弃置批次只摸1，同一其他角色实际回合中的第二个独立批次仍可发动；在已摸牌的获得牌子窗恢复，原帧和实体移动只支付一次。

`MobileZhangXingCaiChecks.QiangwuFrozenJudgmentNativeSlashAndActualPhaseExpiryCold`：真实判定的最终观察子窗、真实高点数杀提交子窗恢复，小/等/大三种点数的距离与次数区别；从真实插入出牌阶段返回同回合正常出牌阶段，旧许可的距离、次数及公开持续效果均到期。

旧 `JudgedRankSlashPolicyChecks.QiangwuActualJudgmentFrozenRankDistanceAndSeparateQuota` 保持OL默认期限及原生支付回归；不改旧断言来迁就移动语义。本文说明实现边界，实际通过情况以批次统一验证结果为准。
