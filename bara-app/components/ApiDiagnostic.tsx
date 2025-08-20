"use client";

import { useState } from "react";
import { apiRequest } from "@/utils/api";

export default function ApiDiagnostic() {
  const [isVisible, setIsVisible] = useState(false);
  const [testResults, setTestResults] = useState<string[]>([]);
  const [isTesting, setIsTesting] = useState(false);

  const addResult = (message: string) => {
    setTestResults(prev => [...prev, `${new Date().toLocaleTimeString()}: ${message}`]);
  };

  const testApiConnectivity = async () => {
    setIsTesting(true);
    setTestResults([]);
    
    const baseUrl = process.env.NEXT_PUBLIC_API_BASE_URL;
    
    addResult("🔍 Starting API connectivity test...");
    addResult(`📡 Base URL: ${baseUrl}`);

    try {
      addResult("🌐 Testing basic connectivity...");
      const response = await fetch(baseUrl || "", {
        method: "GET",
        headers: {
          "ngrok-skip-browser-warning": "true",
        },
      });
      
      const contentType = response.headers.get("content-type");
      addResult(`📋 Content-Type: ${contentType}`);
      addResult(`📊 Status: ${response.status} ${response.statusText}`);
      
      if (contentType?.includes("text/html")) {
        addResult("⚠️ WARNING: Server returned HTML instead of JSON");
        const text = await response.text();
        if (text.includes("ngrok")) {
          addResult("🚨 ISSUE: ngrok session expired or backend not running");
        }
      } else {
        addResult("✅ Server responding with proper content type");
      }
    } catch (error) {
      addResult(`❌ Connection failed: ${error instanceof Error ? error.message : 'Unknown error'}`);
    }

    try {
      addResult("🔧 Testing with API utility...");
      const result = await apiRequest(baseUrl || "");
      if (result.success) {
        addResult("✅ API utility working correctly");
      } else {
        addResult(`⚠️ API utility returned error: ${result.message}`);
      }
    } catch (error) {
      addResult(`❌ API utility failed: ${error instanceof Error ? error.message : 'Unknown error'}`);
    }

    addResult("🏁 Test completed");
    setIsTesting(false);
  };

  if (process.env.NODE_ENV !== 'development') {
    return null;
  }

  return (
    <div className="fixed bottom-4 right-4 z-50">
      {!isVisible ? (
        <button
          onClick={() => setIsVisible(true)}
          className="bg-blue-600 text-white px-3 py-2 rounded-md text-sm hover:bg-blue-700 transition-colors"
          title="API Diagnostic Tool"
        >
          🔧 API Test
        </button>
      ) : (
        <div className="bg-white border border-gray-300 rounded-lg shadow-lg p-4 w-96 max-h-96 overflow-y-auto">
          <div className="flex justify-between items-center mb-3">
            <h3 className="font-semibold text-gray-800">API Diagnostic</h3>
            <button
              onClick={() => setIsVisible(false)}
              className="text-gray-500 hover:text-gray-700"
            >
              ✕
            </button>
          </div>
          
          <div className="space-y-2 mb-3">
            <button
              onClick={testApiConnectivity}
              disabled={isTesting}
              className={`w-full py-2 px-3 rounded text-sm font-medium transition-colors ${
                isTesting
                  ? "bg-gray-300 text-gray-500 cursor-not-allowed"
                  : "bg-blue-600 text-white hover:bg-blue-700"
              }`}
            >
              {isTesting ? "Testing..." : "Test API Connection"}
            </button>
            
            <button
              onClick={() => setTestResults([])}
              className="w-full py-1 px-3 rounded text-sm text-gray-600 hover:bg-gray-100 transition-colors"
            >
              Clear Results
            </button>
          </div>

          {testResults.length > 0 && (
            <div className="bg-gray-50 rounded p-2 max-h-48 overflow-y-auto">
              <div className="text-xs font-mono space-y-1">
                {testResults.map((result, index) => (
                  <div key={index} className="text-gray-700">
                    {result}
                  </div>
                ))}
              </div>
            </div>
          )}

          <div className="mt-3 text-xs text-gray-500">
            <p><strong>Common Issues:</strong></p>
            <ul className="list-disc list-inside space-y-1">
              <li>ngrok session expired</li>
              <li>Backend server not running</li>
              <li>Wrong API endpoint URL</li>
              <li>CORS issues</li>
            </ul>
          </div>
        </div>
      )}
    </div>
  );
}
