# 移动版何太后

核对日期：2026-10-26。正式身份 `mobile:he-tai-hou`，群、女性、三体力，版本 `mobile`，规则集 `sanguosha-mobile`，画像键 `mobile-he-tai-hou`。

[移动版官网何太后92](https://www.sanguosha.cn/pc/hero-detail-92.html)完整HTML含隐藏的戚乱文本。[移动版Wiki何太后](https://wiki.biligame.com/msgs/何太后)身份正文一致：鸩毒仅在其他角色的出牌阶段开始时支付一张手牌，令该角色原生视为使用酒后对其造成一点伤害；戚乱在一名角色的实际回合结束时，可摸自己本回合击杀数的三倍张牌。Wiki身份明确X=击杀数三倍，国战正文则是有过击杀固定摸三张，二者不能混用。

新增 `mobile:zhendu`，只配置其他角色阶段起始，沿既有真实手牌费用、零实体原生酒、额度及伤害返回。与OL每名角色皆可发动、自己用酒不伤害的配置分开。

新增 `mobile:qiluan`：原有 `actualTurnDeathBenefits` 表达式的可选 `ownerKillsOnly` 口径，只计算自己于该实际回合的真实击杀，每人三张；不提供该参数的OL规则继续计算总死亡数加自己击杀数两倍。加载器仅允许该参数与这个原生表达式组合；nullable属性缺省省略，不改变旧配置的内容哈希。真实回合末冻结死亡/击杀事实、原生摸牌帧、子结算和公开AI收益均沿原协议，既不回溯累计以前回合，也不计他杀或无来源体力流失死亡。戚乱在自己和他人的实际回合均可触发。

代表检查区分同回合的本人击杀与无来源死亡、保留OL四张与移动版三张的差别，并用鸩毒在他人实际回合杀死阶段角色，核验回合末的原始计数、恢复和后续回合清零。

独立经典画像为[官网13100](https://www.sanguosha.cn/storage/uploads/images/skins/13100.jpg)，资源文件 `src/CardGame.Wpf/Assets/official-mobile-he-tai-hou.jpg`，由批次统一绑定及核验。
