/**
 * 页面管理器 - 处理模板化页面的加载和渲染
 */
import templateEngine from './template-engine.js';
import componentSystem from './component-system.js';
class PageManager {
    constructor() {
        this.currentPage = null;
        this.loadedTemplates = new Set();
        this.userInfo = null;
        this.init();
    }
    /**
     * 初始化页面管理器
     */
    async init() {
        // 加载用户信息
        this.loadUserInfo();
        // 注册组件
        await this.registerComponents();
        // 加载基础模板
        await this.loadBaseTemplates();
    }
    /**
     * 加载用户信息
     */
    loadUserInfo() {
        try {
            const userInfoStr = sessionStorage.getItem('userInfo') || localStorage.getItem('userInfo');
            if (userInfoStr) {
                this.userInfo = JSON.parse(userInfoStr);
            }
        }
        catch (error) {
            console.error('Failed to load user info:', error);
            this.userInfo = null;
        }
    }
    /**
     * 设置用户信息
     */
    setUserInfo(userInfo) {
        this.userInfo = userInfo;
        if (userInfo) {
            sessionStorage.setItem('userInfo', JSON.stringify(userInfo));
        }
        else {
            sessionStorage.removeItem('userInfo');
            localStorage.removeItem('userInfo');
            localStorage.removeItem('token');
        }
    }
    /**
     * 获取用户信息
     */
    getUserInfo() {
        return this.userInfo;
    }
    /**
     * 注册组件
     */
    async registerComponents() {
        // 注册导航栏组件
        componentSystem.register('navbar', {
            template: await this.loadTemplateContent('templates/components/navbar.html'),
            data: () => ({
                brandName: 'Flyable',
                navItems: this.getNavItems(),
                showUserMenu: !!this.userInfo,
                userInitial: this.userInfo?.username?.charAt(0).toUpperCase() || 'U'
            }),
            methods: {
                toggleUserMenu: function () {
                    const dropdown = document.getElementById('userDropdown');
                    if (dropdown) {
                        dropdown.classList.toggle('show');
                    }
                },
                logout: () => {
                    this.logout();
                }
            }
        });
        // 注册登录表单组件
        componentSystem.register('login-form', {
            template: await this.loadTemplateContent('templates/components/login-form.html'),
            data: () => ({
                brandName: 'Flyable',
                formTitle: '登录',
                formId: 'loginForm',
                showCSRF: true,
                fields: [
                    {
                        id: 'username',
                        name: 'username',
                        type: 'text',
                        placeholder: '用户名',
                        i18nKey: 'login.username',
                        required: true
                    },
                    {
                        id: 'password',
                        name: 'password',
                        type: 'password',
                        placeholder: '密码',
                        i18nKey: 'login.password',
                        required: true,
                        autocomplete: 'off'
                    },
                    {
                        id: 'verifyCode',
                        name: 'verifyCode',
                        type: 'verify-code',
                        placeholder: '验证码',
                        i18nKey: 'login.verifyCode',
                        required: true,
                        imageId: 'verifyCodeImage'
                    }
                ],
                submitText: '登录',
                submitI18nKey: 'login.submit',
                showNavLinks: true,
                navLinks: [
                    {
                        url: 'register.html',
                        text: '还没有账号？立即注册',
                        i18nKey: 'login.noAccount'
                    }
                ]
            })
        });
    }
    /**
     * 获取导航项目
     */
    getNavItems() {
        const items = [
            { url: 'index.html', text: '首页', i18nKey: 'nav.home', active: false },
            { url: 'forum.html', text: '论坛', i18nKey: 'nav.forum', active: false },
            { url: 'novel-management.html', text: '小说管理', i18nKey: 'nav.novels', active: false }
        ];
        // 根据当前页面设置active状态
        const currentPath = window.location.pathname.split('/').pop();
        items.forEach(item => {
            item.active = item.url === currentPath;
        });
        return items;
    }
    /**
     * 加载基础模板
     */
    async loadBaseTemplates() {
        const templates = [
            { name: 'base-layout', url: 'templates/layouts/base.html' }
        ];
        for (const template of templates) {
            if (!this.loadedTemplates.has(template.name)) {
                await this.loadTemplateContent(template.url).then(content => {
                    templateEngine.registerTemplate(template.name, content);
                });
                this.loadedTemplates.add(template.name);
            }
        }
    }
    /**
     * 加载模板内容
     */
    async loadTemplateContent(url) {
        try {
            const response = await fetch(url);
            if (!response.ok) {
                throw new Error(`Failed to load template: ${url}`);
            }
            return await response.text();
        }
        catch (error) {
            console.error(`Error loading template content from ${url}:`, error);
            return '';
        }
    }
    /**
     * 渲染页面
     */
    async renderPage(config) {
        // 调用上一个页面的卸载方法
        if (this.currentPage && config.onUnload) {
            config.onUnload();
        }
        // 准备页面数据
        const pageData = {
            lang: getCurrentLanguage(),
            title: config.title,
            bodyClass: config.bodyClass || '',
            showLanguageSwitch: config.showLanguageSwitch !== false,
            showNavbar: config.showNavbar !== false,
            showFooter: config.showFooter !== false,
            additionalCSS: config.additionalCSS || [],
            additionalJS: config.additionalJS || [],
            customStyles: config.customStyles || '',
            content: '',
            ...config.data
        };
        // 渲染基础布局
        const layout = config.layout || 'base-layout';
        const html = templateEngine.render(layout, pageData);
        // 替换页面内容
        document.documentElement.innerHTML = html;
        // 渲染组件
        await this.renderComponents(config.components || {});
        // 初始化语言系统
        if (typeof updatePageLanguage === 'function') {
            updatePageLanguage();
        }
        // 调用页面加载方法
        if (config.onLoad) {
            config.onLoad();
        }
        this.currentPage = config.title;
    }
    /**
     * 渲染组件
     */
    async renderComponents(components) {
        for (const [selector, componentConfig] of Object.entries(components)) {
            const container = document.querySelector(selector);
            if (container) {
                if (typeof componentConfig === 'string') {
                    // 简单的组件名称
                    componentSystem.create(componentConfig, container);
                }
                else {
                    // 带有属性的组件配置
                    componentSystem.create(componentConfig.name, container, componentConfig.props || {});
                }
            }
        }
        // 渲染默认导航栏
        const navbarContainer = document.getElementById('navbar-container');
        if (navbarContainer) {
            componentSystem.create('navbar', navbarContainer);
        }
    }
    /**
     * 登出用户
     */
    logout() {
        this.setUserInfo(null);
        showSuccess('已成功退出登录');
        setTimeout(() => {
            window.location.href = 'login.html';
        }, 1500);
    }
    /**
     * 检查用户是否已登录
     */
    isLoggedIn() {
        return !!this.userInfo;
    }
    /**
     * 需要登录的页面保护
     */
    requireAuth() {
        if (!this.isLoggedIn()) {
            showError('请先登录');
            setTimeout(() => {
                window.location.href = 'login.html';
            }, 1500);
            return;
        }
    }
}
// 创建全局页面管理器实例
const pageManager = new PageManager();
// 导出到全局作用域
window.pageManager = pageManager;
export default pageManager;
//# sourceMappingURL=page-manager.js.map