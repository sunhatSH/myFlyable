// API_BASE_URL 现在从 config.js 中统一获取

interface ImageVerifyCodeResult {
    imageUrl: string;
    guid: string;
}

async function getVerifyCode(type: string): Promise<string> {
    try {
        const response: Response = await fetch(`${API_BASE_URL}/users/hello_${type}`);
        if (!response.ok) throw new Error('Failed to get verify code');
        return await response.text();
    } catch (error) {
        console.error('Error getting verify code:', error);
        throw error;
    }
}

async function getImageVerifyCode(): Promise<ImageVerifyCodeResult> {
    try {
        const response: Response = await fetch(`${API_BASE_URL}/users/image_verify_code`);
        if (!response.ok) throw new Error('Failed to get image verify code');
        const blob: Blob = await response.blob();
        const guid: string | null = response.headers.get('guid');
        // DEBUG: console.log('Received guid:', guid);
        return {
            imageUrl: URL.createObjectURL(blob),
            guid: guid || ''
        };
    } catch (error) {
        console.error('Error getting image verify code:', error);
        throw error;
    }
}

async function sendVerifyCode(email: string, username: string, guid: string, action: string): Promise<boolean> {
    try {
        const response: Response = await fetch(`${API_BASE_URL}/users/send_verify_code?email=${encodeURIComponent(email)}&username=${encodeURIComponent(username)}&guid=${encodeURIComponent(guid)}&action=${action}`, {
            method: 'POST'
        });
        if (!response.ok) throw new Error('Failed to send verify code');
        return response.status === 200;
    } catch (error) {
        console.error('Error sending verify code:', error);
        throw error;
    }
}

function showMessage(elementId: string, message: string, isError: boolean = false): void {
    const element: HTMLElement | null = document.getElementById(elementId);
    if (!element) return;

    element.textContent = message;
    element.className = isError ? 'error-message' : 'success-message';
    element.style.display = 'block';
}

function hideMessage(elementId: string): void {
    const element: HTMLElement | null = document.getElementById(elementId);
    if (!element) return;

    element.style.display = 'none';
}

function generateGuid(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c: string): string {
        const r: number = Math.random() * 16 | 0;
        const v: number = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function refreshImageVerifyCode(imgElement: HTMLImageElement): void {
    getImageVerifyCode().then(({ imageUrl, guid }) => {
        // DEBUG: console.log('Setting guid:', guid);
        imgElement.src = imageUrl;
        imgElement.dataset.guid = guid;
    }).catch((error: Error) => {
        console.error('Error refreshing image verify code:', error);
    });
} 