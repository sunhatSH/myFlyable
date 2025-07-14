declare const USER_API_BASE_URL: string;
interface ImageVerifyCodeResult {
    imageUrl: string;
    guid: string;
}
declare function getVerifyCode(type: string): Promise<string>;
declare function getImageVerifyCode(): Promise<ImageVerifyCodeResult>;
declare function sendVerifyCode(email: string, username: string, guid: string, action: string): Promise<boolean>;
declare function showMessage(elementId: string, message: string, isError?: boolean): void;
declare function hideMessage(elementId: string): void;
declare function generateGuid(): string;
declare function refreshImageVerifyCode(imgElement: HTMLImageElement): void;
//# sourceMappingURL=user.d.ts.map