# 荀彧·移动版

核查日期：2026-10-26。普通移动身份，魏、男、3体力、神话再临火包，70级/160将魂合成或招募获取。独立 `mobile:quhu` 与 `mobile:jieming` 只组合现有原生节点，没有新运行时、opcode或并行pending。

来源：[移动官网35](https://www.sanguosha.cn/pc/hero-detail-35.html)、[移动BWIKI人物34831](https://wiki.biligame.com/msgs/index.php?title=荀彧&oldid=34831)、[驱虎38636](https://wiki.biligame.com/msgs/index.php?title=驱虎/荀彧&oldid=38636)、[节命20699](https://wiki.biligame.com/msgs/index.php?title=节命/荀彧&oldid=20699)。本次读取当前原始revisions API：人物2025-12-05、驱虎2026-06-05、节命2023-01-04；采用各技能“经典”身份栏，不能混用当前国战节命的一次伤害事件一发动或界移动的摸2后本人条件摸1。官网35完整给出驱虎，节命仅列名称，完整规则由当前普通移动专页补足。

普通驱虎是每个实际出牌阶段一次，拼点目标须有手牌且当前体力大于本人。真实拼点获胜后，由本人从原拼点对手的攻击范围内选择另一角色，伤害来源是原对手；未赢（包括平点）则原对手对本人造成1点伤害。现 `classic:quhu` 的同一组原生拼点/参与者绑定/伤害节点可用，但真实配置 `usesPerTurn:1` 会错误合并同回合的额外出牌阶段，因此不能直接引用。移动定义只改为 `usesPerPhase:1 / usesPerTurn:null`，其余沿现有原帧拼点结果和原生Damage。

普通节命按实际受到的每一点伤害分别可选一名角色，将其手牌补至 `min(5, 该角色当前体力上限)`，不是每次事件只触发一次，也不是一次额外摸至多5张。现 `classic:jieming` 的PerDamagePoint正确，但 `targetMaxHpMinusHandCount` 没有5张上限，不能完整引用。本移动配置保持PerDamagePoint，以已有 `drawToHandCount(amount:5, capAtTargetMaxHp:true)` 准确计算当前目标所需摸牌数。目标已有至少该数量手牌时不摸牌、不弃牌，不把现有手牌强行压回5。

共享 `GameEngine.ConvertingGifts.cs` 的原生DrawToHandCount实际计算 `max(0, min(5, target.MaxHp) - target.HandCount)`，由原生摸牌与获得子窗留在原技能帧续接；没有手工改手牌、复制一份摸牌机制或新建人物pending。驱虎仍用真实两张拼点实体与Typed结果，伤害后的逐点节命以及其摸牌子树按原顺序返回。只新增差异的focused，统一构建与验证由主代理执行。

普通经典图：[3500.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/3500.jpg)，官网35第二张，已与当前移动Wiki“荀彧-经典形象.jpg”视觉核对一致；首张43100为界版。`portraitKey` 为 `mobile-xun-yu`，图鉴分组 `myth-fire`；技能87100/87101，人物及将池87199。
