const API_BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL;

interface AddressDetail {
  street: string;
  city: string;
  state: string;
  country: string;
}

interface VerificationDocument {
  file?: File;
  docType?: string;
}

interface ProducerProfile {
  firstName: string;
  lastName: string;
  middleName?: string;
  email: string;
  password: string;
  phoneNumber: string;
  dateOfBirth: string;
  gender: string | number;
  addressDetail: AddressDetail;
  verificationDocument?: VerificationDocument;
  bio?: string;
}

export async function createProducerProfile(producer: ProducerProfile) {
  const formData = new FormData();

  // basic details
  formData.append("FirstName", producer.firstName);
  formData.append("LastName", producer.lastName);
  formData.append("MiddleName", producer.middleName ?? "");
  formData.append("Email", producer.email);
  formData.append("Password", producer.password);
  formData.append("PhoneNumber", producer.phoneNumber);
  formData.append("DateOfBirth", producer.dateOfBirth); // send as yyyy-mm-dd
  formData.append("Gender", producer.gender.toString()); // assuming enum is int/string

  // address detail (flatten nested object)
  formData.append("AddressDetail.Street", producer.addressDetail.street);
  formData.append("AddressDetail.City", producer.addressDetail.city);
  formData.append("AddressDetail.State", producer.addressDetail.state);
  formData.append("AddressDetail.Country", producer.addressDetail.country);

  // verification document (file upload)
  if (producer.verificationDocument?.file) {
    formData.append(
      "VerificationDocument.File",
      producer.verificationDocument.file
    );
  }
  if (producer.verificationDocument?.docType) {
    formData.append(
      "VerificationDocument.DocType",
      producer.verificationDocument.docType
    );
  }

  // optional bio
  if (producer.bio) {
    formData.append("Bio", producer.bio);
  }

  const res = await fetch(`${API_BASE_URL}/producers/add`, {
    method: "POST",
    body: formData,
    // ❌ do NOT set Content-Type here → fetch auto-sets correct multipart/form-data with boundary
    credentials: "include", // if you need cookies
  });

  if (!res.ok) {
    throw new Error(`Failed to create producer: ${res.statusText}`);
  }

  return res.json();
}