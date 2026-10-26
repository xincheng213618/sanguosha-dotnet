# 司马懿·移动版

核查日期：2026-10-26。当前普通移动身份，魏、男、3体力，标准包；反馈独立编排已有的来源选牌与实体移动操作，鬼才完整引用 `classic:guicai`。没有复制界版每点伤害触发，也没有新增运行时。

来源：[移动官网16](https://www.sanguosha.cn/pc/hero-detail-16.html)、[移动BWIKI人物34805](https://wiki.biligame.com/msgs/index.php?title=司马懿&oldid=34805)（2025-12-05）、[普通反馈20125](https://wiki.biligame.com/msgs/index.php?title=反馈/司马懿&oldid=20125)、[普通鬼才20127](https://wiki.biligame.com/msgs/index.php?title=鬼才/司马懿&oldid=20127)（后二者2022-12-01）。本次再次直接读取当前原始修订；经典栏列出25级获取，属于实际已上线普通身份。

普通反馈在一次伤害事件后可获得伤害来源的一张牌；伤害值为2仍只触发一次。来源的合法区域是Hand和Equipment，判定区不属于其拥有的牌，不能选取。伤害来源可以是本人。现有 `classic:feedback` 的次数与区域相同，但未启用 `allowSameSource`，实际选择/提交会拒绝本人为伤害来源；现有 `boundary:feedback` 启用该能力，却按每点伤害触发，不能整技能等价引用。

因此 `mobile:feedback` 仅以现有DSL声明 `afterDamageApplied/subject:owner/perDamage`，执行 `selectSourceCard`（HE、`allowSameSource:true`）再 `moveBoundCards→ownerHand`。其他人的暗手牌仍是原生隐匿牌位，装备是公开原实体，目标与伤害来源保持原伤害窗口身份，获得仍走原帧实体移动及子返回；没有为同源情况虚构弃牌、复制实体或添加平行pending。

普通鬼才仅允许一张手牌，在原生判定牌生效前替换；不引用界鬼才的HE费用。它继续使用既有改判顺序、每次判定的单次响应以及最终判定实体，不改变闪电伤害的无来源归属。

本次只是现有能力的新配置组合，不新增重复定义快照。共享 `BoundarySimaYiMobileChecks.SelfSourceFeedbackObtainsOwnEquipmentAndReturnsPaidGainCold` 已覆盖同源伤害、自己的真实装备获得、付款后获得子窗cold；`DamageProgramChecks.GenericDamageChoicesClaimSelectGiftDrawAndReplay` 的普通反馈支路已有暗手实体取得和原帧恢复。移动版保留普通perDamage，与界版perDamagePoint明确分开；需要定向时可沿既有机制检查，不复制人物运行时。本次未自行构建或运行检查。

普通经典立绘：[官网1600.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/1600.jpg)，为官网16列表第二张；首张31200为界版图。`portraitKey` 为 `mobile-sima-yi`，本地目标名 `official-mobile-sima-yi.jpg`，图鉴分组 `standard`，技能注册84500、人物及将池84599。没有新增操作码。
