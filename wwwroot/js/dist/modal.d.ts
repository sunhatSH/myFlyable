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
declare class Modal {
    private overlay;
    private modal;
    private queue;
    private isShowing;
    private createModal;
    show(options: ModalOptions): void;
    hide(): void;
    success(message: string, title?: string, autoClose?: boolean): void;
    error(message: string, title?: string, autoClose?: boolean): void;
    warning(message: string, title?: string, autoClose?: boolean): void;
    info(message: string, title?: string, autoClose?: boolean): void;
    confirm(message: string, title?: string, onConfirm?: (() => void) | null, onCancel?: (() => void) | null): void;
    loading(message?: string, title?: string): void;
    custom(options: ModalOptions): void;
}
declare const modal: Modal;
declare const originalConfirm: ((message?: string | undefined) => boolean) & typeof confirm;
//# sourceMappingURL=modal.d.ts.map