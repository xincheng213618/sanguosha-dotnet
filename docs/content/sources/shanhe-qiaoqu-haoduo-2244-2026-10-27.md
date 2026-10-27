# 巧取豪夺2244：借刀父子用牌的加伤归属

[当前公开战法表](https://s.xiaochao.org/data/tactics.json)的2244为巧取豪夺，稀有、原价300铜币、锦囊流（schoolId=9）：因你的【借刀杀人】使用的【杀】伤害+1。当前S5山河册没有固定2244节点，按默认商店和战利品库存投放，不虚构研习节点。

本轮固定战法表SHA-256：`209ce819d57fc8985308376ecc24fe12d2c3ca94dec75b775578f34543774c0e`。

加伤只匹配真实借刀CardUse父帧明确拥有的Slash子CardUse。借刀者、实际用杀者和实体供牌者分别保留原生身份；CardActionContext.RequesterSeat表示供牌委托，不能将其当作借刀者。资格以父帧BorrowedSword续体的ActiveAttackOwnerId、子杀ParentActionId及双方原始actor共同核对，不绑定已经可能因流离等能力改变的当前受害者。

规则复用伤害修正的borrowedSwordUseOwner来源范围和cardDamage层，保留原生伤害封顶、替代及派生链。首次火杀获得加成，铁索传导继承已经形成的伤害，不再次领取巧取豪夺。子杀的source与provider均不改写，后续独立杀也不继承临时加成。没有新增并行付款、帧状态或独立伤害事件。

原生普通实体、丈八、多牌转化、激将供牌以及已有零材料用杀入口都使用同一BorrowedSword续体登记实际子杀；父子识别复用该续体，未为本战法另写用杀流程。聚焦检查覆盖真实实体火杀与铁索、原借刀者和出杀者的持有者区分，以及后续独立实体杀。
