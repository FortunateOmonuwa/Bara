"use client";

import { useState } from "react";
import Logo from "@/components/Logo";
export default function ProfilePage() {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    company: "",
    nin: "",
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
    //  API call or navigation after saving profile
  };

  return (
    <div className="min-h-screen flex justify-center items-start bg-[white] p-6">
      <form
        onSubmit={handleSubmit}
        className="w-full max-w-5xl rounded-lg shadow-lg p-6 space-y-6"
      >
        {/* Header */}
        <div>
          <Logo />
          <h1 className="text-2xl font-semibold text-gray-800 mb-1">
            Set up your profile
          </h1>
        </div>

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
            onChange={handleChange}
            placeholder="NIN"
            className="border p-3 rounded-md border-[#ABADB2]"
          />
        </div>

        {/* Row 4: Country/State/City */}
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

        {/* Row 5: Address details */}
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

        {/* File Uploads */}

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

        {/* <div className="border-dashed border-2 rounded-md p-4 text-center">
            <p className="text-gray-500">Drag and drop file (png, jpeg) here</p>
            <p className="text-red-600 font-medium cursor-pointer mt-2">
              <label>
                Browse
                <input
                  type="file"
                  accept="image/png,image/jpeg"
                  className="hidden"
                  onChange={(e) => handleFileChange(e, "fileAddress")}
                />
              </label>
            </p>
          </div> */}

        {/* Submit */}
        <button
          type="submit"
          className="w-full bg-[#800000] text-white py-3 rounded-md font-medium hover:bg-[#BF0000]"
        >
          Get started
        </button>
      </form>
    </div>
  );
}
