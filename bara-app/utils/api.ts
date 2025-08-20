
interface ApiRequestOptions extends RequestInit {
  skipNgrokWarning?: boolean;
}

interface ApiResponse<T = any> {
  success: boolean;
  data?: T;
  message?: string;
  statusCode?: number;
}


export async function apiRequest<T = any>(
  url: string,
  options: ApiRequestOptions = {}
): Promise<ApiResponse<T>> {
  const { skipNgrokWarning = true, headers = {}, ...restOptions } = options;

  const requestHeaders: HeadersInit = {
    "Content-Type": "application/json",
    ...headers,
  };

  if (skipNgrokWarning) {
    (requestHeaders as Record<string, string>)["ngrok-skip-browser-warning"] = "true";
  }

  try {
    const response = await fetch(url, {
      ...restOptions,
      headers: requestHeaders,
    });

    const contentType = response.headers.get("content-type");
    const isHtml = contentType?.includes("text/html");

    if (isHtml) {
      const htmlText = await response.text();
      
      if (htmlText.includes("ngrok")) {
        throw new Error("ngrok session may have expired or backend server is not running");
      } else if (htmlText.includes("<!DOCTYPE")) {
        throw new Error("Server returned HTML instead of JSON - check if API endpoint is correct");
      } else {
        throw new Error("Unexpected HTML response from server");
      }
    }

    let responseData;
    try {
      responseData = await response.json();
    } catch (parseError) {
      throw new Error("Invalid JSON response from server");
    }

    if (response.ok) {
      return {
        success: true,
        data: responseData,
        statusCode: response.status,
      };
    } else {
      return {
        success: false,
        message: responseData.message || `Request failed with status ${response.status}`,
        statusCode: response.status,
        data: responseData,
      };
    }
  } catch (error) {
    const errorMessage = error instanceof Error ? error.message : "Unknown error occurred";
    
    return {
      success: false,
      message: errorMessage,
      statusCode: 0, 
    };
  }
}


export const api = {

  register: async (data: { Email: string; Password: string; Type: string }) => {
    const baseUrl = process.env.NEXT_PUBLIC_API_BASE_URL;
    const registerUrl = process.env.NEXT_PUBLIC_REGISTER_URL || "/api/user/register";
    
    return apiRequest(`${baseUrl}${registerUrl}`, {
      method: "POST",
      body: JSON.stringify(data),
    });
  },

  verifyEmail: async (email: string, otp: string) => {
    const baseUrl = process.env.NEXT_PUBLIC_API_BASE_URL;
    const verifyUrl = process.env.NEXT_PUBLIC_VERIFY_EMAIL;
    
    return apiRequest(`${baseUrl}${verifyUrl}/${email}/${otp}`, {
      method: "PUT",
    });
  },

  resendVerificationToken: async (email: string) => {
    const baseUrl = process.env.NEXT_PUBLIC_API_BASE_URL;
    const resendUrl = process.env.NEXT_PUBLIC_RESEND_VERIFICATION_TOKEN;
    
    return apiRequest(`${baseUrl}${resendUrl}/${email}`, {
      method: "POST",
    });
  },
};


export const API_ERROR_MESSAGES = {
  NETWORK_ERROR: "Network error - please check your internet connection",
  NGROK_EXPIRED: "Development server connection expired - please restart the backend",
  INVALID_JSON: "Server returned invalid response - please try again",
  SERVER_ERROR: "Server error - please try again later",
  UNKNOWN_ERROR: "Something went wrong - please try again",
};
