# 甘宁·移动版

> 此版本与 `classic:gan-ning` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核查日期：2026-10-26。当前普通移动身份，吴、男、4体力，标准包；完整引用现有 `classic:qixi`，没有复制技能定义或运行时。

来源：[移动官网9](https://www.sanguosha.cn/pc/hero-detail-9.html)、[移动BWIKI人物35671](https://wiki.biligame.com/msgs/index.php?title=甘宁&oldid=35671)（2026-01-08）、[普通奇袭39419](https://wiki.biligame.com/msgs/index.php?title=奇袭/甘宁&oldid=39419)（2026-07-03）。本次直接读取以上当前MediaWiki修订；普通经典栏给出17级等级奖励，属于实际已上线的普通标准身份。技能页当前经典正文仍为出牌阶段以一张黑色牌作过河拆桥，没有另加国战或界版条件。

已有 `_shared/Skills/conversion-cutover.rules.json` 的 `classic:qixi` 使用单一真实实体转换：来源区域明确包括Hand和Equipment，黑桃/梅花匹配，输出Dismantlement，`forPlay:true/forResponse:false`。没有阶段或回合次数上限，不限基本牌，不是仅黑色手牌。提交时沿原生转换校验有效花色、实际材料位置和当前用牌资格，装备区材料也要真实移出装备区，不能伪装成手牌成本。

转换后保留原生过河拆桥目标资格、目标手牌隐匿牌位及装备/判定区选择、实体丢弃和无懈响应流程；这是一张使用的普通锦囊牌，不是直接绕过牌使用流程弃置他人牌。实际费用及装备离区子窗继续由原CardUse帧和既有typed返回承载，不重复支付原实体。普通版本没有界甘宁的奋威，也没有附加摸牌收益。

纯配置沿用既有加载、注册及转换机制检查，不新增重复人物夹具。已有 `WushuangModeChecks.PurchasedBlackEquipmentQixiFinishesItsExactDestroyedPaymentCold` 直接使用经典奇袭的真实装备材料，在原生过河拆桥目标选牌窗cold并完成一次实际弃牌；它同时验证材料离开装备后的销毁不会使原已支付锦囊丢失来源或再次付款。本次仅读取既有覆盖，没有执行构建或测试。

普通经典立绘：[官网900.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/900.jpg)，为官网9普通皮肤列表首图；30500为界版图，不作本普通身份立绘。`portraitKey` 为 `mobile-gan-ning`，本地目标名 `official-mobile-gan-ning.jpg`，图鉴分组 `standard`，人物注册及将池序号84299。没有新增操作码。
