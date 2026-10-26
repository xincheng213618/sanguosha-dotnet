# 普通移动版袁绍

核查日期：2026-10-26。`mobile:yuan-shao` 共用 `character:yuan-shao`，群、男、4体力，神话再临火包；普通身份不混入界、谋或国战版本。

## 来源

- [移动官网武将41](https://www.sanguosha.cn/index.php/pc/hero-detail-41.html)：两张同花色手牌转为万箭；列出血裔名称但没有对应技能正文，玩法文字明确主公时其他群角色提高本人手牌上限。
- [人物35794](https://wiki.biligame.com/msgs/index.php?title=袁绍&oldid=35794)：普通4体力、神话再临火包，开服上线，经典画师SoniaTang。
- [乱击28363](https://wiki.biligame.com/msgs/index.php?title=乱击/袁绍&oldid=28363)、[血裔28629](https://wiki.biligame.com/msgs/index.php?title=血裔/袁绍&oldid=28629)：本次实际重读当前 MediaWiki revisions API 的原始正文和修订标识，分别为2024-08-23及2024-10-16修订；人物页为2026-01-10修订。浏览工具不能读取Wiki raw入口，使用API取得正文，没有把入口失败当成规则证据。

## 乱击与有效花色

普通规则是出牌阶段用两张同花色手牌当万箭，不是国战的每种花色每回合一次，也没有界移动乱击的响应摸牌和未伤害补牌。手牌中的装备牌可以作材料，装备区的实体不能当成手牌。

独立 `mobile:luanji` 沿既有 `useSelectedCardsAs` 和原生万箭，启用现有 `viewAs.useEffectiveInputSuit:true`。候选配对、主动费用可用性、提交重验及原生全局用牌的最终重验均按本人当前真实有效花色比较。经典 `classic:luanji` 未启用此可选项，仍保留原默认行为；没有以同名引用来声称补齐其历史印刷花色限制。

本人合法候选的费用花色通过可空只读映射投影给AI和界面，让获得红颜后的黑桃与红桃手牌可以组成同有效花色费用。该映射在初始化时拷贝冻结，只作本人预览；命令没有它，Core重新读取实体和当前花色，不信任投影作为支付凭据。默认映射为null，其他主动技能的旧形状和默认比较不变。

两张材料仍是原始实体，经Hand进入Processing，保留同一个真实 `CardUseFrame`、原生无懈及逐目标闪响应，最终typed返回原主动帧。没有新增人物pending、旁路伤害或重复万箭执行器。无每阶段、每回合或每种花色的额外次数限制。

聚焦行为以真实移动袁绍在原生山河设置获得红颜为例，核对混印刷同有效花色手牌、装备区排除、真实材料与原万箭子窗cold；WPF只补实际费用草稿确认的代表覆盖。共享经典原生乱击、AI费用和主动花色检查继续承担默认行为回归，未为人物复制定义快照或整个恢复矩阵。

## 血裔

完整引用 `classic:xueyi` 的 `factionHandLimitBonus`、群、主公资格及系数2。手牌上限每次查询动态数当前其他存活群角色，读取真实有效势力，并由当前有效技能来源和原生主公技资格控制。本人的群身份不计入，死者不计入，失去或失效的来源不继续贡献；没有开局人数快照、界版“裔”标记或新的数值实现。

## 素材

普通经典图为[官网4100.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/4100.jpg)，官网41第二张；首图43800是界版。`portraitKey=mobile-yuan-shao`、gallery `myth-fire`，技能86300、人物及当前将池86399。图源和正式身份分别核对，不按官网第一张图片推断版本。
