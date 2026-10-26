# 普通移动孙策：激昂、魂姿、制霸

核对日期：2026-10-26。`mobile:sun-ce`，吴、男、4体力，神话再临·山，开服武将。

## 当前来源

- [移动官网54](https://www.sanguosha.cn/pc/hero-detail-54.html)：实际HTML完整列三技能及普通魂姿派生英姿/英魂。普通图采用[5400.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/5400.jpg)，不是首张界版48100；portrait `mobile-sun-ce`、图鉴 `myth-mountain`。
- [人物修订34813](https://wiki.biligame.com/msgs/index.php?title=孙策&oldid=34813)，2025-12-05，普通吴男4、山包、开服、290将魂/招募。
- [激昂修订38473](https://wiki.biligame.com/msgs/index.php?title=激昂/孙策&oldid=38473)，2026-05-11：使用决斗/红杀指定目标后，或成为其目标后，可摸1。
- [魂姿修订31601](https://wiki.biligame.com/msgs/index.php?title=魂姿/孙策&oldid=31601)，2025-07-09：准备阶段恰1体力觉醒，减1上限，获得普通英姿/英魂。
- [制霸修订20173](https://wiki.biligame.com/msgs/index.php?title=制霸/孙策&oldid=20173)，2022-12-01：其他吴角色自己的出牌阶段限一次，主动与主公拼点；已觉醒的主公可拒绝；对方没赢时主公可获得两张原拼点牌。

本轮通过移动BWIKI revisions API重新读取原始wikitext、修订号与时间，并实读官网HTML。规则不引用国战鹰扬/魂殇、界魂姿两体力条件或界英姿的手牌上限收益。

## 激昂与魂姿

现 `classic:jiang` 实际编排晚至 `cardUseBeforeTargetEffects`，红杀集合也不含雷/冰，不能直接作为当前普通移动激昂。独立 `mobile:jiang` 在原生 `cardUseTargetsFinalized` 的实际用牌者/目标入口分别判断决斗或红色完整四杀族，再调用既有原生Draw1。没有逐额外目标重复摸牌、阶段限次或把打出牌当成使用牌。

魂姿直接引用完整 `classic:hunzi`：每局一次，准备阶段体力恰1，先原生减1上限再通过共享技能变更授予 `classic:yingzi`、`classic:yinghun`。不额外回复，不把界版的“两体力可觉醒”或锁定英姿手牌上限带进来。

## 制霸的最小可选声明

独立 `mobile:zhiba` 复用已注册 `resolveBoundarySunCeZhiba` 原生挑战操作，仅显式声明 `skillIds:[classic:hunzi]` 作为拒绝资格的觉醒依据。描述符只允许最多一个此类引用，内容注册器核其存在且标有Awakening；省略时仍是原界移动魂姿默认，不引入新模型、版本或epoch。

挑战动作按已启用且声明该操作的技能/activation能力索引产生；仍由其他吴角色在自己的实际Play发起，Lord来源走共享原资格，付出的是真实手牌。每provider/owner/技能/activation的既有阶段账本保持。觉醒完成依据在原挑战帧发行时冻结，随后询问/原生拼点/可选领牌均使用原帧和现有typed Pindian返回。平局属于未赢；取得的是仍可领取的两张真实拼点实体，不额外造牌。

默认界版没有新增声明字段，旧空列表及原魂姿依据保持；完成领取与失源后的已付拼点仍按原技能帧身份、来源实例和内容hash匹配，不加中央人物名单或平行pending。

## 聚焦验证范围

新增检查仅覆盖实际红雷杀的actor/target早窗口及一次原摸牌子窗Cold，普通魂姿完成后声明的拒绝依据、真实原生拼点领取与拒绝Cold，以及可选觉醒引用的数量/存在/类型准入。原界制霸完整矩阵继续复用 `Mobile boundary Sun Ce: Wu Lord tie claim awakening refusal cold`，不重复其全部边界。
