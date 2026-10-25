# 许攸·移动版

核对日期：2026-10-26。独立普通身份 `mobile:xu-you`，群势力，男性，3体力，神话再临阴包。采用已经正式上线的移动身份规则，不混用国战版本或其他产品的同名技能。

规则来源：[移动官网254](https://www.sanguosha.cn/pc/hero-detail-254.html)、[移动版BWIKI许攸](https://wiki.biligame.com/msgs/许攸)（[固定版本 oldid=35828](https://wiki.biligame.com/msgs/index.php?title=许攸&oldid=35828)，页面更新2026-01-10）。BWIKI普通身份页列出2019-05-08上线与元宝、竞价获取方式，当前普通经典栏提供完整三技能正文。

## 成略

每个出牌阶段限一次，转换技从阳开始：阳摸一张牌后弃置两张手牌，阴摸两张牌后弃置一张手牌；之后，本实际回合使用与真实弃牌花色相同的牌无距离和次数限制。官网和移动BWIKI的期限均为“本回合”，因此不能直接引用原 `classic:chenglue` 的原出牌阶段许可。

独立 `mobile:chenglue` 复用 `alternatingSuitDrawDiscard` 原语，添加可选 `allowanceExpiry: "actualTurnEnd"`。真实摸牌、逐张选弃、一次原生逻辑弃置及移牌子窗均继续保存在原技能帧；许可在真实弃置后按当时有效花色签发，不使用之后的牌区或花色重算已支付的材料。签发事实和玩家视图的既有许可记录追加可空 `Expiry` 与 `ActualTurnOwnerSeat`，冻结实际回合编号和拥有者；同一实际回合中的后续出牌阶段及结束阶段等原生实际用牌均可消费这项许可，不额外要求当前阶段为Play或当前角色就是许可持有者。原实际回合游标返回时明确失效，进入下一实际回合也不能复用。失去成略不撤销已经签发且未到期的规则许可。

省略新策略时仍按原 `PhaseInstanceId` 到期；新增的可空字段为空时不写入JSON，保留经典技能原定义、原期限及默认序列化。策略只允许 `actualTurnEnd`，不能给不相干的恃才操作附加期限。继续使用现有回合期限枚举，不新增平行待决对象、规则epoch或格式版本。

## 恃才与寸目

恃才完整引用 `classic:shicai`：在一个实际回合中首次使用某类别牌的结算完成时，可将该牌置于牌堆顶并摸一张牌。原定义按实际用牌类别和原生完成事实判断，保留基本、锦囊、装备三个类别；装备以实际装备后仍可主张的实体为材料，不把技能失去或响应包装当作新的首次用牌。移动正文与该现有定义一致。

寸目完整引用 `classic:cunmu`，摸牌改从牌堆底取得；原生摸牌请求、摸牌后移动和恢复均沿用共享流程。两项技能不复制JSON或新增重复人物定义检查。

## 定向验证与素材

`MobileXuYouChecks.ActualTurnSuitsSurviveExtraPlayAndSourceLossAfterPaidCold` 使用固定全黑桃真实杀牌堆，比较默认经典期限与移动实际回合期限；在原生弃牌子窗冷恢复后失去成略，检验同一实际回合的另一出牌阶段可真实连续使用两张远距离杀，而下一实际回合不再享有旧许可。`LoaderRejectsUnsupportedAllowanceDeadlinesAndUnrelatedNode` 覆盖不支持的期限与不相关操作拒绝。构建和执行结果由批次统一验证记录。

`IssuedActualTurnAllowanceSupportsEndingUseUntilCursorReturns` 另复用同一小夹具，在自己的额外Play中签发许可，实际进入自己的结束阶段后冷恢复；通过其他角色的原生用杀请求，使用匹配花色的实体杀攻击真实距离超出攻击范围的角色，比较经典许可已过期与移动许可仍生效，并验证实际游标返回后的许可投影为空。它直接覆盖旧Play条件会错误收窄的非出牌阶段边界，不为夹具创建当前通用插入阶段尚未支持的跨角色临时切座流程。

经典立绘采用[移动官网42100](https://www.sanguosha.cn/storage/uploads/images/skins/42100.jpg)，资源键 `mobile-xu-you`，图鉴归入神话再临阴包 `myth-yin`。官网另列42102、42103、42101皮肤，不替代经典原画。
