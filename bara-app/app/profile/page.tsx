"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Logo from "@/components/Logo";
import Image from "next/image";

export default function ProfilePage() {
  const router = useRouter();

  const [activeTab, setActiveTab] = useState<
    "personal" | "location" | "identity"
  >("personal");

  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    company: "",
    phone: "",
    nin: "",
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log("Profile Data:", formData);
    router.push("/dashboard");
  };

  return (
    <div className="fixed inset-0 bg-[#1a0000] bg-opacity-80 flex items-center justify-center z-50 p-4">
      <form
        onSubmit={handleSubmit}
        className="bg-white rounded-lg shadow-lg p-10 w-full max-w-3xl space-y-2"
      >
        {/* Logo */}
        <Logo />

        {/* Title */}
        <h1 className="text-xl md:text-2xl font-medium text-[#22242A]">
          Set up your profile
        </h1>

        {/* Tabs */}
        <div className="flex border-b border-gray-300 text-sm font-medium text-gray-500 space-x-6">
          <button
            type="button"
            onClick={() => setActiveTab("personal")}
            className="relative px-4 pt-2 pb-3 text-sm"
          >
            <span
              className={`${
                activeTab === "personal" ? "text-[#810306]" : "text-gray-500"
              }`}
            >
              Personal information
            </span>
            {activeTab === "personal" && (
              <div className="absolute bottom-0 left-1/2 -translate-x-1/2 h-1 w-full bg-[#810306] rounded-tr-2xl rounded-tl-2xl" />
            )}
          </button>

          <button
            type="button"
            onClick={() => setActiveTab("location")}
            className="relative px-4 pt-2 pb-3 text-sm"
          >
            <span
              className={`${
                activeTab === "location" ? "text-[#810306]" : "text-gray-500"
              }`}
            >
              Location details
            </span>
            {activeTab === "location" && (
              <div className="absolute bottom-0 left-1/2 -translate-x-1/2 h-1 w-full bg-[#810306] rounded-tr-2xl rounded-tl-2xl" />
            )}
          </button>

          <button
            type="button"
            onClick={() => setActiveTab("identity")}
            className="relative px-4 pt-2 pb-3 text-sm"
          >
            <span
              className={`${
                activeTab === "identity" ? "text-[#810306]" : "text-gray-500"
              }`}
            >
              Identity verification
            </span>
            {activeTab === "identity" && (
              <div className="absolute bottom-0 left-1/2 -translate-x-1/2 h-1 w-full bg-[#810306] rounded-tr-2xl rounded-tl-2xl" />
            )}
          </button>
        </div>

        {/* Form Sections */}
        {activeTab === "personal" && (
          <>
            {/* Input Fields */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div className="flex flex-col">
                <label
                  htmlFor="firstName"
                  className="text-sm text-[#22242A] font-semibold mb-1"
                >
                  First name
                </label>
                <input
                  type="text"
                  id="firstName"
                  name="firstName"
                  value={formData.firstName}
                  onChange={handleChange}
                  className="border border-[#ABADB2] p-3 rounded-md w-full"
                />
              </div>

              <div className="flex flex-col">
                <label
                  htmlFor="lastName"
                  className="text-sm text-[#22242A] font-semibold mb-1"
                >
                  Last name
                </label>
                <input
                  type="text"
                  id="lastName"
                  name="lastName"
                  value={formData.lastName}
                  onChange={handleChange}
                  className="border border-[#ABADB2] p-3 rounded-md w-full"
                />
              </div>

              <div className="flex flex-col md:col-span-2">
                <label
                  htmlFor="company"
                  className="text-sm text-[#22242A] font-semibold mb-1"
                >
                  Company/Studio
                </label>
                <input
                  type="text"
                  id="company"
                  name="company"
                  value={formData.company}
                  onChange={handleChange}
                  className="border border-[#ABADB2] p-3 rounded-md w-full"
                />
              </div>
            </div>

            {/* Phone and NIN */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div className="flex flex-col">
                <label
                  htmlFor="phone"
                  className="text-sm text-[#22242A] font-semibold mb-1"
                >
                  Phone number
                </label>
                <div className="flex items-center border border-[#ABADB2] rounded-md px-3 py-2">
                  <div className="flex items-center mr-2 border border-[#ABADB2] py-2 px-2 rounded-md">
                    <Image
                      src="/Nigerian flag.png"
                      alt="Nigeria flag"
                      width={20}
                      height={14}
                      className="mr-2"
                    />
                    <span className="text-sm mr-2">+234</span>
                    <Image
                      src="/dropdown.png"
                      alt="Dropdown arrow"
                      width={20}
                      height={12}
                    />
                  </div>
                  <input
                    type="text"
                    id="phone"
                    name="phone"
                    value={formData.phone}
                    onChange={handleChange}
                    className="flex-1 outline-none bg-transparent text-sm"
                  />
                </div>
              </div>

              <div className="flex flex-col">
                <label
                  htmlFor="nin"
                  className="text-sm text-[#22242A] font-semibold mb-1"
                >
                  NIN
                </label>
                <input
                  type="text"
                  id="nin"
                  name="nin"
                  value={formData.nin}
                  onChange={handleChange}
                  className="border border-[#ABADB2] p-3 rounded-md w-full"
                />
              </div>
            </div>
          </>
        )}

        {activeTab === "location" && (
          <div className="text-[#22242A] text-sm">
            <p>Location details form goes here...</p>
          </div>
        )}

        {activeTab === "identity" && (
          <div className="text-[#22242A] text-sm">
            <p>Identity verification form goes here...</p>
          </div>
        )}

        {/* Buttons */}
        {/* Buttons */}
        <div className="flex justify-end gap-4 pt-4">
          <button
            type="button"
            onClick={() => router.push("/dashboard")}
            className="px-8 py-2 border-2 border-[#810306] text-[#810306] rounded-md font-semibold"
          >
            Skip
          </button>
          <button
            type="submit"
            disabled={
              !formData.firstName ||
              !formData.lastName ||
              !formData.company ||
              !formData.phone ||
              !formData.nin
            }
            className={`px-8 py-2 rounded-md font-semibold ${
              formData.firstName &&
              formData.lastName &&
              formData.company &&
              formData.phone &&
              formData.nin
                ? "bg-[#810306] text-white"
                : "bg-[#F5F5F5] text-[#858990] cursor-not-allowed"
            }`}
          >
            Save
          </button>
        </div>
      </form>
    </div>
  );
}
