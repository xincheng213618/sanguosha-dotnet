# 普通移动版庞统

核查日期：2026-10-26。`mobile:pang-tong` 为蜀、男、3体力，神话再临火包，共用 `character:pang-tong`；不混入界版的额外连环目标或出牌阶段主动涅槃，也不授予OL、谋版的衍生技能。

## 当前来源

- [移动官网详情36](https://www.sanguosha.cn/index.php/pc/hero-detail-36.html)：普通连环为梅花手牌转铁索或重铸；普通涅槃仅在本人濒死时可发动，顺序为弃置区域内牌、复原武将牌、摸三张，再回复体力。
- [人物34815](https://wiki.biligame.com/msgs/index.php?title=庞统&oldid=34815)：2025-12-05修订，普通蜀男3、神话再临火包、开服上线，经典形象画师KayaK。
- [连环20686](https://wiki.biligame.com/msgs/index.php?title=连环/庞统&oldid=20686)、[涅槃22257](https://wiki.biligame.com/msgs/index.php?title=涅槃/庞统&oldid=22257)：本次已实际读取 MediaWiki revisions API 的当前原始正文、修订号及时间，分别为2023-01-04与2023-04-13修订。涅槃类型为限定技，当前普通正文的复原、摸牌、回复顺序与官网相符。

## 连环

`mobile:lianhuan` 沿已有真实铁索连环与重铸执行器，只允许当前Hand中的一张有效梅花实体。使用现有 `useEffectiveInputSuit:true`，与旧 `classic:lianhuan` 的印刷花色默认分开；不扩大到装备区，不增加目标数，不制造新伤害或新的材料状态。正常铁索有一至两名合法目标，重铸不产生一次锦囊Use。

## 涅槃的顺序与资格

旧 `classic:niepan` 虽同名，但只解除横置，没有翻回正面，而且先回复再摸牌。界移动版本另有主动入口且复原在摸牌、回复之后。因此普通版本独立配置 `mobile:niepan`，不以引用同名旧实现承诺当前规则。

仅本人真实 `selfDyingResponse` 提供可选发动，原生游戏次数一次；没有出牌阶段按钮。接受后原程序帧依次执行：清理Hand、Equipment及Judgment全部区域牌；解除横置；翻回正面；正常摸三张；最后沿原生回复将体力补至当前上限与三点中的较小值。牌移动、角色状态变化、摸牌及回复产生的子窗仍归原帧，没有人物旁路pending或直接写体力。

聚焦检查针对这项新增顺序：本人实际持有HEJ原实体并横置、背面朝上，在真实濒死接受涅槃后，核对区域费用及复原均先于摸牌，摸牌子窗中仍未提前回复；该子窗cold后只摸三张、只完成一次原生回复和一次限定效果。普通铁索和阶段预算沿共享转换与限定技机制，不复制定义快照或整个组合矩阵。

## 素材与注册

普通经典图为[官网3600.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/3600.jpg)，`portraitKey=mobile-pang-tong`、gallery `myth-fire`；技能87000、87001，人物及当前将池87099。素材与正式身份单独核对，没有改规则格式、全局版本或内容包版本。
