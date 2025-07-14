/**
 * 页面管理器 - 处理模板化页面的加载和渲染
 */
interface PageConfig {
    title: string;
    layout?: string;
    showNavbar?: boolean;
    showFooter?: boolean;
    showLanguageSwitch?: boolean;
    additionalCSS?: string[];
    additionalJS?: string[];
    customStyles?: string;
    bodyClass?: string;
    components?: {
        [key: string]: any;
    };
    data?: any;
    onLoad?: () => void;
    onUnload?: () => void;
}
interface UserInfo {
    userId?: number;
    username?: string;
    email?: string;
}
declare class PageManager {
    private currentPage;
    private loadedTemplates;
    private userInfo;
    constructor();
    /**
     * 初始化页面管理器
     */
    private init;
    /**
     * 加载用户信息
     */
    private loadUserInfo;
    /**
     * 设置用户信息
     */
    setUserInfo(userInfo: UserInfo | null): void;
    /**
     * 获取用户信息
     */
    getUserInfo(): UserInfo | null;
    /**
     * 注册组件
     */
    private registerComponents;
    /**
     * 获取导航项目
     */
    private getNavItems;
    /**
     * 加载基础模板
     */
    private loadBaseTemplates;
    /**
     * 加载模板内容
     */
    private loadTemplateContent;
    /**
     * 渲染页面
     */
    renderPage(config: PageConfig): Promise<void>;
    /**
     * 渲染组件
     */
    private renderComponents;
    /**
     * 登出用户
     */
    logout(): void;
    /**
     * 检查用户是否已登录
     */
    isLoggedIn(): boolean;
    /**
     * 需要登录的页面保护
     */
    requireAuth(): void;
}
declare const pageManager: PageManager;
export default pageManager;
//# sourceMappingURL=page-manager.d.ts.map