# 夏侯渊·移动版

核查日期：2026-10-26。普通移动身份，魏、男、4体力，神话再临风；47级获得。完整引用 `classic:shensu`，不复制技能定义或运行时。

来源：[移动官网28](https://www.sanguosha.cn/pc/hero-detail-28.html)、[普通神速固定版本39416](https://wiki.biligame.com/msgs/index.php?title=神速/夏侯渊&oldid=39416)（2026-07-03）、[人物固定版本35867](https://wiki.biligame.com/msgs/index.php?title=夏侯渊&oldid=35867)。当前普通身份仍为两项：跳过判定与摸牌；或者跳过出牌并弃一张装备。每项均为原生无距离普通杀。国战新版本的第二项已删除弃装备成本，不能套到本普通身份；界版的第三项也不属于本人物。

共享 `judgment-cutover.rules.json` 的 `classic:shensu` 对应实际回合准备流前和摸牌结束两个独立入口。第二项通过实际手牌或装备区的一张装备实体支付，等待原生移牌子结算后跳过出牌并使用原生虚拟杀；第一项实际跳过判定与摸牌。直接引用同一技能保留现有阶段、真实付款、目标距离、响应及原帧恢复合同。

纯内容引用由共享加载、注册和机制检查覆盖；集成复用 `Character turned over Shensu three branches and hand equipment payment` 检查共用阶段与装备成本原语，不新增重复人物夹具。该代表回归不把界版第三项赋给本人物。

经典立绘来自[官网2800.jpg](https://www.sanguosha.cn/storage/uploads/images/skins/2800.jpg)，本地 `official-mobile-xiahou-yuan.jpg`，图鉴分组 `myth-wind`。
