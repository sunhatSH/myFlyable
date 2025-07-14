/**
 * 通用弹窗系统
 */

interface ModalButton {
    text: string;
    type?: string;
    onClick?: () => void;
    closeOnClick?: boolean;
}

interface ModalOptions {
    type?: 'success' | 'error' | 'warning' | 'info' | 'confirm';
    title?: string;
    message?: string;
    buttons?: ModalButton[];
    autoClose?: boolean;
    autoCloseTime?: number;
}

interface ModalIcons {
    [key: string]: string;
}

class Modal {
    private overlay: HTMLElement | null = null;
    private modal: HTMLElement | null = null;
    private queue: ModalOptions[] = [];
    private isShowing: boolean = false;

    // 创建弹窗HTML结构
    private createModal(): void {
        if (this.overlay) return;

        this.overlay = document.createElement('div');
        this.overlay.className = 'modal-overlay';
        this.overlay.innerHTML = `
            <div class="modal">
                <div class="modal-icon"></div>
                <div class="modal-title"></div>
                <div class="modal-content"></div>
                <div class="modal-buttons"></div>
            </div>
        `;
        document.body.appendChild(this.overlay);
        this.modal = this.overlay.querySelector('.modal');

        // 点击遮罩层关闭弹窗
        this.overlay.addEventListener('click', (e: MouseEvent) => {
            if (e.target === this.overlay) {
                this.hide();
            }
        });

        // ESC键关闭弹窗
        document.addEventListener('keydown', (e: KeyboardEvent) => {
            if (e.key === 'Escape' && this.isShowing) {
                this.hide();
            }
        });
    }

    // 显示弹窗
    show(options: ModalOptions): void {
        this.createModal();

        if (this.isShowing) {
            this.queue.push(options);
            return;
        }

        this.isShowing = true;
        const {
            type = 'info',
            title = '',
            message = '',
            buttons = [],
            autoClose = false,
            autoCloseTime = 3000
        } = options;

        if (!this.modal) return;

        // 设置弹窗类型
        this.modal.className = `modal ${type}`;

        // 设置图标
        const iconElement = this.modal.querySelector('.modal-icon') as HTMLElement;
        const icons: ModalIcons = {
            success: '✅',
            error: '❌',
            warning: '⚠️',
            info: 'ℹ️',
            confirm: '❓'
        };
        iconElement.textContent = icons[type] || icons.info;

        // 设置标题
        const titleElement = this.modal.querySelector('.modal-title') as HTMLElement;
        titleElement.textContent = title;
        titleElement.style.display = title ? 'block' : 'none';

        // 设置内容
        const contentElement = this.modal.querySelector('.modal-content') as HTMLElement;
        contentElement.innerHTML = message;

        // 设置按钮
        const buttonsContainer = this.modal.querySelector('.modal-buttons') as HTMLElement;
        buttonsContainer.innerHTML = '';

        if (buttons.length === 0) {
            // 默认确定按钮
            const defaultButton = document.createElement('button');
            defaultButton.className = 'modal-btn';
            defaultButton.textContent = '确定';
            defaultButton.onclick = () => this.hide();
            buttonsContainer.appendChild(defaultButton);
        } else {
            buttons.forEach((button: ModalButton) => {
                const btnElement = document.createElement('button');
                btnElement.className = `modal-btn ${button.type || ''}`;
                btnElement.textContent = button.text;
                btnElement.onclick = () => {
                    if (button.onClick) {
                        button.onClick();
                    }
                    if (button.closeOnClick !== false) {
                        this.hide();
                    }
                };
                buttonsContainer.appendChild(btnElement);
            });
        }

        // 显示弹窗
        this.overlay!.classList.add('show');

        // 自动关闭
        if (autoClose) {
            this.modal.classList.add('auto-close');
            setTimeout(() => {
                this.hide();
            }, autoCloseTime);
        }
    }

    // 隐藏弹窗
    hide(): void {
        if (!this.overlay || !this.isShowing) return;

        this.overlay.classList.remove('show');
        this.modal!.classList.remove('auto-close');

        setTimeout(() => {
            this.isShowing = false;
            // 处理队列中的下一个弹窗
            if (this.queue.length > 0) {
                const nextModal = this.queue.shift()!;
                setTimeout(() => this.show(nextModal), 100);
            }
        }, 300);
    }

    // 成功提示
    success(message: string, title: string = '成功', autoClose: boolean = true): void {
        this.show({
            type: 'success',
            title,
            message,
            autoClose,
            autoCloseTime: 2000
        });
    }

    // 错误提示
    error(message: string, title: string = '错误', autoClose: boolean = false): void {
        this.show({
            type: 'error',
            title,
            message,
            autoClose
        });
    }

    // 警告提示
    warning(message: string, title: string = '警告', autoClose: boolean = false): void {
        this.show({
            type: 'warning',
            title,
            message,
            autoClose
        });
    }

    // 信息提示
    info(message: string, title: string = '提示', autoClose: boolean = true): void {
        this.show({
            type: 'info',
            title,
            message,
            autoClose,
            autoCloseTime: 2500
        });
    }

    // 确认对话框
    confirm(message: string, title: string = '确认', onConfirm: (() => void) | null = null, onCancel: (() => void) | null = null): void {
        this.show({
            type: 'confirm',
            title,
            message,
            buttons: [
                {
                    text: '确定',
                    onClick: onConfirm || undefined
                },
                {
                    text: '取消',
                    type: 'secondary',
                    onClick: onCancel || undefined
                }
            ]
        });
    }

    // 加载提示
    loading(message: string = '加载中...', title: string = ''): void {
        this.show({
            type: 'info',
            title,
            message: `<div class="modal-loading"></div>${message}`,
            buttons: []
        });
    }

    // 自定义弹窗
    custom(options: ModalOptions): void {
        this.show(options);
    }
}

// 创建全局实例
const modal = new Modal();

// 全局快捷方法
(window as any).showSuccess = (message: string, title?: string, autoClose?: boolean) => modal.success(message, title, autoClose);
(window as any).showError = (message: string, title?: string, autoClose?: boolean) => modal.error(message, title, autoClose);
(window as any).showWarning = (message: string, title?: string, autoClose?: boolean) => modal.warning(message, title, autoClose);
(window as any).showInfo = (message: string, title?: string, autoClose?: boolean) => modal.info(message, title, autoClose);
(window as any).showConfirm = (message: string, title?: string, onConfirm?: (() => void) | null, onCancel?: (() => void) | null) => modal.confirm(message, title, onConfirm, onCancel);
(window as any).showLoading = (message?: string, title?: string) => modal.loading(message, title);
(window as any).hideModal = () => modal.hide();

// 替换原有的alert函数
(window as any).alert = (message: string) => {
    modal.info(message, '提示');
};

// 替换原有的confirm函数 - 保持原有的同步行为
const originalConfirm = window.confirm;
(window as any).confirmAsync = (message: string): Promise<boolean> => {
    return new Promise((resolve) => {
        modal.confirm(message, '确认',
            () => resolve(true),
            () => resolve(false)
        );
    });
};

// 兼容原有的showMessage函数
(window as any).showMessage = (elementId: string, message?: string, isError: boolean = false): void => {
    if (typeof elementId === 'string' && !message) {
        // 如果第一个参数是消息内容
        if (isError) {
            modal.error(elementId);
        } else {
            modal.success(elementId);
        }
    } else {
        // 原有格式
        if (isError) {
            modal.error(message || '');
        } else {
            modal.success(message || '');
        }
    }
}; 