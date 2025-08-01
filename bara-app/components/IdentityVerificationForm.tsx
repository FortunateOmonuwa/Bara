"use client";

import { useRef, useState, useEffect } from "react";
import Image from "next/image";

const options = [
  "National identity number",
  "International passport",
  "Driver’s license",
];

export default function IdentityVerificationForm() {
  const [documentType, setDocumentType] = useState("");
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const [uploadedFile, setUploadedFile] = useState<File | null>(null);
  const [uploadComplete, setUploadComplete] = useState(false);
  const fileInputRef = useRef<HTMLInputElement | null>(null);
  const [previewUrl, setPreviewUrl] = useState<string | null>(null);

  const handleSelect = (option: string) => {
    setDocumentType(option);
    setDropdownOpen(false);
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file && (file.type === "image/png" || file.type === "image/jpeg")) {
      setUploadedFile(file);
      setUploadComplete(true);
      const url = URL.createObjectURL(file);
      setPreviewUrl(url);
    }
  };

  const handleBrowseClick = () => {
    fileInputRef.current?.click();
  };

  useEffect(() => {
    return () => {
      // Clean up preview URL to avoid memory leaks
      if (previewUrl) URL.revokeObjectURL(previewUrl);
    };
  }, [previewUrl]);

  return (
    <div className="space-y-6 pt-2">
      {/* Document Type Dropdown */}
      <div className="relative">
        <label className="block mb-1 text-sm font-semibold text-[#22242A]">
          Proof of identity
        </label>
        <div
          className="relative cursor-pointer"
          onClick={() => setDropdownOpen(!dropdownOpen)}
        >
          <input
            type="text"
            placeholder="Select document type"
            className="w-full border border-[#ABADB2] p-3 rounded-md pr-10 text-sm text-[#22242A] placeholder:text-[#9CA3AF] cursor-pointer focus:!border-[#800000] focus:outline-none"
            value={documentType}
            readOnly
          />
          <Image
            src="/dropdown.png"
            alt="Dropdown icon"
            width={20}
            height={12}
            className="absolute right-3 top-1/2 -translate-y-1/2 pointer-events-none"
          />
        </div>

        {dropdownOpen && (
          <div className="absolute mt-1 w-full bg-white border border-[#ABADB2] rounded-md shadow-lg z-10">
            {options.map((option) => (
              <div
                key={option}
                className="flex items-center px-4 py-2 text-sm cursor-pointer hover:bg-[#F5F5F5]"
                onClick={() => handleSelect(option)}
              >
                <div className="w-4 h-4 mr-2 rounded-full border border-[#ABADB2] flex items-center justify-center">
                  {documentType === option && (
                    <div className="w-2 h-2 bg-[#800000] rounded-full" />
                  )}
                </div>
                {option}
              </div>
            ))}
          </div>
        )}
      </div>

      {/* Upload Section */}
      <div>
        <label className="block mb-1 text-sm font-semibold text-[#22242A]">
          Upload selected proof of identity
        </label>

        {uploadComplete && uploadedFile && previewUrl ? (
          <div className="border-2 border-dashed border-[#ABADB2] rounded-md p-4 bg-[#F5F5F5]">
            <div className="flex flex-col items-center space-y-3">
              <Image
                src="/checkring.png"
                alt="Upload complete"
                width={32}
                height={32}
              />
              <span className="text-sm text-[#333740] font-medium">
                Upload complete
              </span>
              <Image
                src={previewUrl}
                alt="Uploaded preview"
                width={60}
                height={100}
                className="rounded-sm border"
              />
              <div className="w-full h-1 bg-green-600 rounded" />
            </div>
          </div>
        ) : (
          <div
            className="w-full h-40 border-2 border-dashed border-[#ABADB2] rounded-md bg-[#F5F5F5] flex flex-col items-center justify-center text-center cursor-pointer hover:bg-gray-100 transition"
            onClick={handleBrowseClick}
          >
            <p className="text-sm text-[#333740]">
              Drag and drop file (png, jpeg) here
            </p>
            <p className="text-sm text-[#333740] mt-1">
              or{" "}
              <span className="text-[#810306] font-semibold underline">
                Browse
              </span>
            </p>
            {uploadedFile && (
              <p className="mt-2 text-xs text-[#555]">{uploadedFile.name}</p>
            )}
          </div>
        )}

        <input
          type="file"
          accept="image/png, image/jpeg"
          onChange={handleFileChange}
          ref={fileInputRef}
          className="hidden"
        />
      </div>
    </div>
  );
}
