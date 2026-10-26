# 华佗·移动版

> 此版本与 `classic:hua-tuo` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核查日期：2026-10-26。当前普通移动身份，群、男、3体力，标准包；完整引用现有 `classic:qingnang`、`classic:jijiu`，没有复制技能定义或运行时。

来源：[移动官网22](https://www.sanguosha.cn/pc/hero-detail-22.html)、[移动BWIKI人物34803](https://wiki.biligame.com/msgs/index.php?title=华佗&oldid=34803)（2025-12-05）、[普通青囊28185](https://wiki.biligame.com/msgs/index.php?title=青囊/华佗&oldid=28185)（2024-08-14）、[普通急救20504](https://wiki.biligame.com/msgs/index.php?title=急救/华佗&oldid=20504)（2023-01-03）。本次直接读取当前原始修订；普通经典栏给出41级获取，属于已上线标准身份。

官网默认只显示急救，HTML的隐藏第二技能栏完整写出青囊：每出牌阶段限一次，弃一张手牌，然后令一名已受伤角色回复1点体力。当前百科简写为令一名角色回复1点体力，没有添加额外收益或解除一般回复资格。既有角色Content实际为青囊引用 `phase-owned-card-actions`，注册的是revision2：Hand一张费用、`usesPerPhase:1/usesPerTurn:null`、`anyWounded`目标，原生弃牌后回复1。它允许为自己或其他性别的受伤角色回复，不限红牌，也不因本回合已有一次出牌阶段就否定额外Play的首次青囊。仓库另存的旧 `classic-qingnang-skills` 文件不是本次引用的注册来源，不能据其旧回合限次作判断。

急救完整引用 `conversion-cutover` 的红色HE实体转换：手牌或装备区的一张红桃/方块牌，在本人回合外按真实桃使用；没有阶段或回合次数上限，没有把装备伪装成手牌材料。普通回合内不能以此代替青囊，自己的回合外濒死时仍按实际回合归属判断资格。原桃的实际受救者、实体付款、装备离区子窗、原生恢复和濒死返回保持既有协议，不改为直接回复或无实体摸牌。

普通青囊没有界版红色费用后可再次发动的条件，普通急救也没有神华佗的其他效果。纯人物配置沿用共享加载、注册、阶段限次、转换与原生回复检查，不新增重复定义快照或人物夹具。本次没有自行构建或运行检查。

普通经典立绘：[官网2200.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/2200.jpg)，为官网22列表第二张；首图31800为界华佗，不能作本普通身份立绘。`portraitKey` 为 `mobile-hua-tuo`，本地目标名 `official-mobile-hua-tuo.jpg`，图鉴分组 `standard`，人物与将池序号84699。没有新增操作码。
