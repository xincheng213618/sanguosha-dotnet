# 普通移动曹丕：行殇、放逐、颂威

核对日期：2026-10-26。`mobile:cao-pi` 为普通移动身份，魏、男、3体力，神话再临·林。

## 当前来源与版本

- [移动官网43](https://www.sanguosha.cn/pc/hero-detail-43.html)：普通曹丕，列行殇、放逐、颂威；实际HTML技能正文仅提供行殇，其余两技用移动BWIKI当前原始修订补齐，不将官网不完整正文当作三技完整证据。
- [人物修订39996](https://wiki.biligame.com/msgs/index.php?title=曹丕&oldid=39996)，2026-08-01：魏男3，林包，95级、160将魂或招募获取。经典上线时间栏为空，不臆造首发日期。
- [行殇修订20629](https://wiki.biligame.com/msgs/index.php?title=行殇/曹丕&oldid=20629)，2023-01-04：其他角色死亡时，可获得其所有牌。
- [放逐修订33204](https://wiki.biligame.com/msgs/index.php?title=放逐/曹丕&oldid=33204)，2025-09-20：每次受伤后，可令一名其他角色先翻面，再摸本人已损失体力值张牌。
- [颂威修订26607](https://wiki.biligame.com/msgs/index.php?title=颂威/曹丕&oldid=26607)，2024-03-28：主公技，其他魏角色最终黑色判定牌生效后，由该角色选择是否令主公摸1。

上述修订由移动BWIKI revisions API读取原始wikitext、修订号与时间核实。普通立绘采用[官方4300.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/4300.jpg)，不是官网首张界版45000；portrait `mobile-cao-pi`，图鉴分组 `myth-forest`。

## 行殇的实际来源边界

[规则集区域术语镜像](https://gltjk.com/sanguosha/rules/glossary/zone.html)说明一般操作“角色的牌”指其拥有的手牌和装备牌；判定区牌及武将牌上的牌不属于这一集合。

当前 `classic:xingshang` 省略死亡清理来源区过滤，会沿历史默认读取整个原死亡清理账册；该账册还含判定区、不屈及公私堆，不能直接作为普通移动完整身份的同义技能。

独立 `mobile:xingshang` 仅使用一个既有 `claimDeathCleanupCards` 节点，显式限定 `hand/equipment`。资格及实际领取均读取原死亡帧中当前仍可领取的真实HE实体，不领取J或武将牌堆，不新建死亡清理、副账册或平行pending。原生移动及获得子结算仍在原程序帧，恢复后不再次领取。没有回复体力、额外摸牌、费用或每回合一次限制。

显式来源过滤的领取节点通过已有 `PendingMovementContinuation` 等待真实获得子窗；证明使用同一死亡窗口、原清理实体、原移动序列与领取来源，返回仍走已有类型化移动续接。资格预检只作用于首个无条件且显式过滤的领取节点；省略来源区的历史定义保留旧准入和直接继续，后置条件分支也不被当成初始费用来预检。

## 完整复用的两技

`classic:fangzhu` 的真实DSL已是 `afterDamageApplied/perDamage`，选择其他存活角色，原生翻面后再摸X；X沿原生命周期冻结事实。一次2点伤害只提供一次放逐，不按点重复。普通移动版没有目标弃牌/失去体力的备选，不使用界版摸牌后翻面的流程。

`boundary:songwei-mobile` 与普通颂威正文及行为相同，直接复用该完整已注册ID：保留Lord标签和共享资格，核对原最终判定、其他存活魏角色，由实际判定者可选回答，再进行主公原生摸牌。没有另加主公同意窗口，也不因复用ID继承界版行殇或放逐。经典曹丕实际未注册颂威，不能漏掉第三技。

## 验证范围

新增focused只检查普通行殇原HE实体、判定区排除以及真实领取子窗的代表冷恢复。既有死亡清理和移动界颂威机制过滤继续复用；不复制完整界版死亡二选一、颂威、放逐或跨技矩阵。加载、内容注册与整批验证由统一验证流程负责。
