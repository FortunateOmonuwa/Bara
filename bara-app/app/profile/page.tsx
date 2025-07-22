"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Logo from "@/components/Logo";

export default function ProfilePage() {
  const router = useRouter();

  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    company: "",
    nin: "",
    altNin: "",
    phone: "",
    country: "Nigeria",
    state: "",
    city: "",
    houseNumber: "",
    street: "",
    zip: "",
    proofIdentity: "",
    proofAddress: "",
    fileIdentity: null as File | null,
    fileAddress: null as File | null,
  });

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleFileChange = (
    e: React.ChangeEvent<HTMLInputElement>,
    key: "fileIdentity" | "fileAddress"
  ) => {
    const file = e.target.files?.[0] || null;
    setFormData((prev) => ({ ...prev, [key]: file }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log("Profile Data: ", formData);

    // ✅ Navigate to landing page after form submit
    router.push("/landing"); // change to "/" if your landing page is the home page
  };

  return (
    <div className="fixed inset-0 bg-[#1a0000] bg-opacity-50 flex items-center justify-center z-50 p-4">
      <form
        onSubmit={handleSubmit}
        className="bg-white rounded-lg shadow-lg p-6 w-full max-w-5xl max-h-[90%] overflow-y-auto space-y-6"
      >
        {/* Header */}
        <Logo />
        <h1 className="text-2xl font-semibold text-gray-800 mb-1">
          Set up your profile
        </h1>

        {/* Row 1 */}
        <div className="grid md:grid-cols-2 gap-4">
          <input
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            placeholder="First name"
            className="border p-3 rounded-md border-[#ABADB2]"
          />
          <input
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            placeholder="Last name"
            className="border p-3 rounded-md border-[#ABADB2]"
          />
        </div>

        {/* Row 2 */}
        <div className="grid md:grid-cols-2 gap-4">
          <input
            name="company"
            value={formData.company}
            onChange={handleChange}
            placeholder="Company/studio"
            className="border p-3 rounded-md border-[#ABADB2]"
          />
          <input
            name="nin"
            value={formData.nin}
            onChange={handleChange}
            placeholder="NIN"
            className="border p-3 rounded-md border-[#ABADB2]"
          />
        </div>

        {/* Row 3 */}
        <div className="grid md:grid-cols-2 gap-4">
          <input
            name="phone"
            value={formData.phone}
            onChange={handleChange}
            placeholder="Phone number"
            className="border p-3 rounded-md border-[#ABADB2]"
          />
          <input
            name="altNin"
            value={formData.altNin}
            onChange={handleChange}
            placeholder="Alternate NIN"
            className="border p-3 rounded-md border-[#ABADB2]"
          />
        </div>

        {/* Row 4 */}
        <div className="grid md:grid-cols-3 gap-4">
          <select
            name="country"
            value={formData.country}
            onChange={handleChange}
            className="border p-3 rounded-md border-[#ABADB2]"
          >
            <option value="Nigeria">🇳🇬 Nigeria</option>
            <option value="Ghana">🇬🇭 Ghana</option>
          </select>
          <input
            name="state"
            value={formData.state}
            onChange={handleChange}
            placeholder="State/province"
            className="border p-3 rounded-md border-[#ABADB2]"
          />
          <input
            name="city"
            value={formData.city}
            onChange={handleChange}
            placeholder="City"
            className="border p-3 rounded-md border-[#ABADB2]"
          />
        </div>

        {/* Row 5 */}
        <div className="grid md:grid-cols-3 gap-4">
          <input
            name="houseNumber"
            value={formData.houseNumber}
            onChange={handleChange}
            placeholder="House number"
            className="border p-3 rounded-md border-[#ABADB2]"
          />
          <input
            name="street"
            value={formData.street}
            onChange={handleChange}
            placeholder="Street"
            className="border p-3 rounded-md border-[#ABADB2]"
          />
          <input
            name="zip"
            value={formData.zip}
            onChange={handleChange}
            placeholder="Zip code"
            className="border p-3 rounded-md border-[#ABADB2]"
          />
        </div>

        {/* Proof of Identity & Address */}
        <div className="grid md:grid-cols-2 gap-4">
          <select
            name="proofIdentity"
            value={formData.proofIdentity}
            onChange={handleChange}
            className="border p-3 rounded-md border-[#ABADB2]"
          >
            <option value="">Select proof of identity</option>
            <option value="nin">National ID</option>
            <option value="passport">Passport</option>
          </select>
          <select
            name="proofAddress"
            value={formData.proofAddress}
            onChange={handleChange}
            className="border p-3 rounded-md border-[#ABADB2]"
          >
            <option value="">Select proof of address</option>
            <option value="utility">Utility Bill</option>
            <option value="bank">Bank Statement</option>
          </select>
        </div>

        {/* File Upload */}
        <div className="border-dashed border-[#ABADB2] border-2 rounded-md p-10 text-center bg-[#F5F5F5]">
          <p className="text-gray-500">Drag and drop file (png, jpeg) here</p>
          <p className="text-red-600 font-medium cursor-pointer mt-2">
            <label>
              Browse
              <input
                type="file"
                accept="image/png,image/jpeg"
                className="hidden"
                onChange={(e) => handleFileChange(e, "fileIdentity")}
              />
            </label>
          </p>
        </div>

        {/* Submit Button */}
        <button
          type="submit"
          className="w-full bg-[#800000] text-white py-3 rounded-md font-medium hover:bg-[#1a0000] transition-all duration-300 ease-in-out"
        >
          Get started
        </button>
      </form>
    </div>
  );
}
