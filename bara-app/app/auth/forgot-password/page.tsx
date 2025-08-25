"use client";

import { useState } from "react";
import Image from "next/image";

export default function ForgotPasswordPage() {
  const [email, setEmail] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const handleForgotPassword = async () => {
    if (!email) return;
    setIsLoading(true);

    // simulate API call
    setTimeout(() => {
      setIsLoading(false);
      alert("Password reset link sent to " + email);
    }, 1500);
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-[#1a0000]">
      <div className="bg-white rounded-lg shadow-md w-full max-w-xl p-8">
        {/* Logo */}
        <div className="flex justify-center mb-6">
          <Image src="/logo.png" alt="Bara Logo" width={60} height={40} />
        </div>

        {/* Inner content wrapper to control width */}
        <div className="w-full max-w-sm mx-auto">
          {/* Title */}
          <h1 className="text-xl font-semibold text-[#22242A] mb-2">
            Forgot password
          </h1>
          <p className="text-sm text-[#333740] mb-6">
            Don’t worry, we’ll send you a message to help you reset your
            password.
          </p>

          {/* Email Input */}
          <label className="block text-sm font-medium text-[#22242A] mb-2">
            Email
          </label>
          <input
            type="email"
            placeholder="Enter your email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="w-full border border-[#ABADB2] rounded-md px-3 py-3 mb-4 bg-white 
              focus:outline-none focus:ring-1 focus:ring-[#800000] focus:border-[#800000]"
          />

          {/* Continue Button */}
          <button
            type="button"
            onClick={handleForgotPassword}
            disabled={!email || isLoading}
            className={`w-full font-medium py-3 rounded-md flex items-center justify-center transition-colors ${
              email && !isLoading
                ? "bg-[#800000] text-white hover:bg-[#1a0000]"
                : "bg-[#F5F5F5] text-[#858990] cursor-not-allowed"
            }`}
          >
            {isLoading ? "Sending..." : "Continue"}
          </button>
        </div>
      </div>
    </div>
  );
}
