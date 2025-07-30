"use client";

import { useState } from "react";
import Image from "next/image";

const documentOptions = [
  "National identity number",
  "International passport",
  "Driver’s license",
];

export default function IdentityVerificationForm() {
  const [documentType, setDocumentType] = useState("");
  const [dropdownOpen, setDropdownOpen] = useState(false);

  const handleSelect = (option: string) => {
    setDocumentType(option);
    setDropdownOpen(false);
  };

  return (
    <div className="text-[#22242A] text-sm relative">
      <form className="space-y-6">
        {/* Document Type */}
        <div className="relative">
          <label className="block mb-1 font-medium">Proof of identity</label>
          <div
            className="relative cursor-pointer"
            onClick={() => setDropdownOpen((prev) => !prev)}
          >
            <input
              type="text"
              placeholder="Select document type"
              className="w-full border border-[#D1D5DB] rounded-md px-3 py-2 pr-10 placeholder:text-[#9CA3AF] text-[#111827] bg-white"
              value={documentType}
              readOnly
            />
            {/* Dropdown Icon */}
            <Image
              src="/dropdown.png"
              alt="Dropdown Icon"
              width={20}
              height={12}
              className="absolute right-3 top-1/2 -translate-y-1/2 pointer-events-none"
            />
          </div>

          {/* Dropdown Modal */}
          {dropdownOpen && (
            <div className="absolute z-10 mt-1 w-full bg-white border border-[#D1D5DB] rounded-md shadow-md">
              {documentOptions.map((option) => (
                <div
                  key={option}
                  onClick={() => handleSelect(option)}
                  className="flex items-center gap-2 px-3 py-2 hover:bg-gray-100 cursor-pointer"
                >
                  <div className="w-4 h-4 rounded-full border border-gray-400 flex items-center justify-center">
                    {documentType === option && (
                      <div className="w-2 h-2 bg-black rounded-full" />
                    )}
                  </div>
                  <span className="text-sm">{option}</span>
                </div>
              ))}
            </div>
          )}
        </div>

        {/* Upload Section */}
        <div>
          <label className="block mb-1 font-medium">
            Upload selected proof of identity
          </label>
          <div className="w-full h-40 border-2 border-dashed border-[#D1D5DB] rounded-lg bg-[#F9FAFB] flex items-center justify-center text-center cursor-pointer hover:bg-[#F3F4F6] transition">
            <p className="text-[#22242A] font-medium mb-1">
              Drag and drop file
            </p>
            <p className="text-[#6B7280] text-sm">
              (png, jpeg) here or{" "}
              <span className="text-blue-600 underline">Browse</span>
            </p>
          </div>
        </div>
      </form>
    </div>
  );
}
