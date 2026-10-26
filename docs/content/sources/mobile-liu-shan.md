# 刘禅·移动版

核查日期：2026-10-26。普通移动身份，蜀、男、3体力、神话再临山包，开服上线，100级/260将魂合成或招募获取。享乐及若愚为独立移动JSON，放权完整引用 `classic:fangquan`；不把界放权体力上限手牌上限、界若愚附加收益等混入普通版。

来源：[官网53](https://www.sanguosha.cn/pc/hero-detail-53.html)、[当前移动人物36814](https://wiki.biligame.com/msgs/index.php?title=刘禅&oldid=36814)、[享乐38720](https://wiki.biligame.com/msgs/index.php?title=享乐/刘禅&oldid=38720)、[放权20737](https://wiki.biligame.com/msgs/index.php?title=放权/刘禅&oldid=20737)、[若愚26610](https://wiki.biligame.com/msgs/index.php?title=若愚/刘禅&oldid=26610)、[派生激将38985](https://wiki.biligame.com/msgs/index.php?title=激将/刘禅&oldid=38985)。本次成功读取当前原始revisions API：人物2026-02-19，享乐2026-06-06，放权2023-01-05，若愚2024-03-28，激将2026-06-11。网页缓存曾呈现享乐旧20736，以本次API的38720为准；编辑时间不冒充上线改动公告。官网列出三技能名称，正文读取只有享乐/若愚，缺少的完整放权正文由本次API20737补齐，不把列名当成完整规则证据。

享乐在成为杀目标后，让实际使用者弃一张基本牌，或令此杀对刘禅无效。`classic:xiangle` 的原生支付/无效节点可复用，但其实际CardUseBeforeTargetEffects窗口较晚；普通 `mobile:xiangle` 在CardUseTargetsFinalized、Target关系下执行，列出普通/火/雷/冰四杀，不等待后来的目标效果或响应前收益。真实基本牌费用来自原使用者手牌，经过既有逻辑弃牌及移牌子窗；拒绝支付时在同一原CardUse父帧标记**仅刘禅目标**无效，不移除目标、不取消整张牌或影响其他目标。

现有NullifyCurrentCardEffect原生操作仅将上下文准入扩至目标拥有者的Finalized或原BeforeTargetEffects，继续核对同一ActionId、ParentCardUseFrameId、ActorSeat、EffectiveKind及该拥有者确为真实目标；沿原IneffectiveTargetSeats在后续效果阶段跳过。这不是以修改目标列表代替“对你无效”。经典技能的旧窗口保持。

放权仍在出牌阶段开始前可跳过，然后本实际回合结束时可弃一张手牌，令另一角色执行一个额外回合。`classic:fangquan` 现有声明、真实Hand费用、选其他角色、PendExtraTurn及自然座位返回完全匹配。其AfterNormalDraw是原生进入Play前的窗口，不是“必须实际摸过牌”：`CompleteTurnStartAfterDraw` 对SkipDraw仍经过该窗口，仅SkipPlay时不再开放，因此真实被跳过摸牌仍可放权，已因乐不思蜀跳Play则不可重复发动。跳过时无手牌仍可声明，只有回合末缺手牌时没有后续费用/收益。普通版没有界放权以MaxHP作为本回合手牌上限的收益。

若愚保持主公资格、每局一次、准备阶段当前体力全场最小（含并列），按原生顺序加1上限、回复1、授予激将。不能完整引用 `classic:ruoyu`，因为它授予 `classic:jijiang`，后者历史配置整回合限1；当前普通派生激将没有这一次数限制。独立 `mobile:ruoyu` 仅将授予数组改为已注册的 `boundary:jijiang-mobile`，该技能与普通派生激将完整等价：真实其他蜀角色依次提供杀、原生使用/响应仍归请求者、主公资格与自然杀次数保留，没有界版额外收益。技能变化沿共享GrantSkills及原帧typed返回，不另造觉醒机制。

普通经典图：[5300.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/5300.jpg)，官网53第二张；与当前移动Wiki“刘禅-经典形象.jpg”已视觉核对一致。首张48300为界版。`portraitKey` 为 `mobile-liu-shan`，图鉴 `myth-mountain`；技能87200/87201，人物及将池87299。只为享乐新增精确时序行为覆盖；放权、觉醒/授予及激将沿既有共享检查，统一验证由主代理执行。
