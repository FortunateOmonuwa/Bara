"use client";

import Image from "next/image";
import { useState } from "react";

export default function LocationForm() {
  const [form, setForm] = useState({
    country: "Nigeria",
    state: "",
    city: "",
    houseNumber: "",
    street: "",
    zipCode: "",
  });

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  return (
    <div className="text-[#22242A] text-sm mt-8">
      {/* First row */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-4">
        {/* Country */}
        <div className="flex flex-col">
          <label htmlFor="country" className="mb-1 font-medium">
            Country
          </label>
          <div className="relative">
            <div className="absolute inset-y-0 left-3 flex items-center pointer-events-none">
              <Image
                src="/Nigerian flag.png"
                alt="Nigeria flag"
                width={20}
                height={14}
              />
            </div>

            <select
              id="country"
              name="country"
              value={form.country}
              onChange={handleChange}
              className="w-full border border-[#ABADB2] rounded-md px-10 py-2 appearance-none bg-white text-sm"
            >
              <option value="Nigeria">Nigeria</option>
              <option value="Ghana">Ghana</option>
            </select>

            {/* Dropdown arrow */}
            <Image
              src="/dropdown.png"
              alt="Dropdown"
              width={20}
              height={12}
              className="absolute right-3 top-1/2 transform -translate-y-1/2 pointer-events-none"
            />
          </div>
        </div>

        {/* State */}
        <div className="flex flex-col">
          <label htmlFor="state" className="mb-1 font-medium">
            State/province
          </label>
          <input
            id="state"
            name="state"
            value={form.state}
            onChange={handleChange}
            className="w-full border border-[#ABADB2] rounded-md px-3 py-2"
          />
        </div>

        {/* City */}
        <div className="flex flex-col">
          <label htmlFor="city" className="mb-1 font-medium">
            City
          </label>
          <input
            id="city"
            name="city"
            value={form.city}
            onChange={handleChange}
            className="w-full border border-[#ABADB2] rounded-md px-3 py-2"
          />
        </div>
      </div>

      {/* Second row */}
      <div className="grid grid-cols-1 md:grid-cols-12 gap-4">
        {/* House number - 3 columns */}
        <div className="flex flex-col md:col-span-3">
          <label htmlFor="houseNumber" className="mb-1 font-medium">
            House number
          </label>
          <input
            id="houseNumber"
            name="houseNumber"
            value={form.houseNumber}
            onChange={handleChange}
            className="w-full border border-[#ABADB2] rounded-md px-3 py-2"
          />
        </div>

        {/* Street - 6 columns */}
        <div className="flex flex-col md:col-span-6">
          <label htmlFor="street" className="mb-1 font-medium">
            Street
          </label>
          <input
            id="street"
            name="street"
            value={form.street}
            onChange={handleChange}
            className="w-full border border-[#ABADB2] rounded-md px-3 py-2"
          />
        </div>

        {/* Zip code - 3 columns */}
        <div className="flex flex-col md:col-span-3">
          <label htmlFor="zipCode" className="mb-1 font-medium">
            Zip code
          </label>
          <input
            id="zipCode"
            name="zipCode"
            value={form.zipCode}
            onChange={handleChange}
            className="w-full border border-[#ABADB2] rounded-md px-3 py-2"
          />
        </div>
      </div>
    </div>
  );
}
