"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Image from "next/image";
import EyeToggle from "@/components/EyeToggle";

export default function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const router = useRouter();

  const baseUrl = process.env.NEXT_PUBLIC_API_BASE_URL;
  const loginUrl = process.env.NEXT_PUBLIC_LOGIN_USER;

  const canLogin = email.trim() !== "" && password.trim() !== "";

  const handleLogin = async () => {
    if (!canLogin) return;
    setIsLoading(true);

    try {
      const response = await fetch(`${baseUrl}${loginUrl}`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "ngrok-skip-browser-warning": "true",
        },
        body: JSON.stringify({
          Email: email,
          Password: password,
        }),
      });

      const resBody = await response.json();

      if (response.ok) {
        localStorage.setItem("userId", resBody.data.userId);
        localStorage.setItem("userType", resBody.data.userType);
        router.push("/dashboard");
      } else {
        console.error("Login failed:", resBody);
      }
    } catch (error) {
      console.error("Login error:", error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <main className="min-h-screen flex items-center justify-center bg-[#1a0000] px-4">
      {/* White container card */}
      <div className="bg-white rounded-lg shadow-lg flex flex-col md:flex-row w-full max-w-5xl h-[600px] overflow-hidden">
        {/* Left: 40% width full-bleed image */}
        <div className="hidden md:block md:w-2/5 relative">
          <Image
            src="/family.png"
            alt="Login Illustration"
            fill
            className="object-cover object-left-top"
            priority
          />
        </div>

        {/* Right: Form Section (60%) */}
        <div className="flex-1 md:w-3/5 flex flex-col justify-center items-center px-6 md:px-12 overflow-y-auto">
          <div className="w-full max-w-sm">
            {" "}
            {/* Logo */}
            {/* Logo (left-aligned) */}
            <div className="mb-4 self-start">
              <Image src="/logo.png" alt="Bara Logo" width={70} height={40} />
            </div>
            {/* Heading (left-aligned) */}
            <h1 className="text-2xl font-semibold mb-8 text-[#22242A] text-left">
              Log in
            </h1>
            {/* Google Login */}
            <button
              type="button"
              className="w-full bg-[#800000] text-white font-medium py-3 rounded-md hover:bg-[#1a0000] flex items-center justify-center gap-4 mb-6"
            >
              <Image
                src="/Google.png"
                alt="Google Icon"
                width={20}
                height={20}
              />
              Log in with Google
            </button>
            <div className="flex items-center justify-center my-4">
              <span className="text-sm text-[#333740]">or</span>
            </div>
            {/* Email */}
            <label className="block text-sm font-medium text-[#22242A] mb-2">
              Email
            </label>
            <input
              type="email"
              placeholder="Enter your email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="w-full border border-[#ABADB2] rounded-md px-3 py-3 mb-4 bg-white focus:outline-none focus:ring-1 focus:ring-[#800000] focus:border-[#800000]"
            />
            {/* Password */}
            <label className="block text-sm font-medium text-[#22242A] mb-2">
              Password
            </label>
            <div className="relative mb-4">
              <input
                type={showPassword ? "text" : "password"}
                placeholder="Enter your password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="w-full border border-[#ABADB2] rounded-md px-3 py-3 pr-10 bg-white
          focus:outline-none focus:ring-1 focus:ring-[#800000] focus:border-[#800000]"
              />
              <EyeToggle
                isVisible={showPassword}
                onToggle={() => setShowPassword((p) => !p)}
              />
            </div>
            {/* Forgot Password + Actions */}
            <div className="flex items-center justify-between text-sm mb-4">
              <a
                href="/auth/forgot-password"
                className="text-[#333740] font-medium text-medium"
              >
                Forgot your password?
              </a>
            </div>
            {/* Login Button */}
            <button
              type="button"
              onClick={handleLogin}
              disabled={!canLogin || isLoading}
              className={`w-full font-medium py-3 rounded-md flex items-center justify-center gap-2 transition-colors ${
                canLogin && !isLoading
                  ? "bg-[#800000] text-white hover:bg-[#1a0000]"
                  : "bg-[#F5F5F5] text-[#858990] cursor-not-allowed"
              }`}
            >
              {isLoading ? "Logging in..." : "Log in"}
              {!isLoading && <span className="ml-2 text-lg">→</span>}
            </button>
            {/* Create Account */}
            <p className="text-sm text-center mt-6 text-[#333740] font-medium text-medium">
              Don’t have an account?{" "}
              <a
                href="/auth/register"
                className="text-[#810306] underline font-medium text-medium"
              >
                Create account
              </a>
            </p>
          </div>
        </div>
      </div>
    </main>
  );
}
