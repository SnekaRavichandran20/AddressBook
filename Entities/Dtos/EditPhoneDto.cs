/* 
 * Swagger AddressBook - OpenAPI 3.0
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.0.11
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace AddressBookApi.Dtos
{
    /// <summary>
    /// EditPhoneDto
    /// </summary>
    [DataContract]
    public partial class EditPhoneDto : IEquatable<EditPhoneDto>
    {
        /// <summary>
        /// Gets or Sets OldPhoneNumber
        /// </summary>
        [DataMember(Name = "old_phone_number", EmitDefaultValue = false)]
        public string? OldPhoneNumber { get; set; }

        /// <summary>
        /// Gets or Sets OldPhoneNumber
        /// </summary>
        [DataMember(Name = "new_phone_number", EmitDefaultValue = false)]
        public string? NewPhoneNumber { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string? Type { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class EditPhoneDto {\n");
            sb.Append("  OldPhoneNumber: ").Append(OldPhoneNumber).Append("\n");
            sb.Append("  NewPhoneNumber: ").Append(NewPhoneNumber).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as EditPhoneDto);
        }

        /// <summary>
        /// Returns true if EditPhoneDto instances are equal
        /// </summary>
        /// <param name="input">Instance of EditPhoneDto to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(EditPhoneDto input)
        {
            if (input == null)
                return false;

            return
                (
                    this.OldPhoneNumber == input.OldPhoneNumber ||
                    (this.OldPhoneNumber != null &&
                    this.OldPhoneNumber.Equals(input.OldPhoneNumber))
                ) &&
                (
                    this.NewPhoneNumber == input.NewPhoneNumber ||
                    (this.NewPhoneNumber != null &&
                    this.NewPhoneNumber.Equals(input.NewPhoneNumber))
                ) &&
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.OldPhoneNumber != null)
                    hashCode = hashCode * 59 + this.OldPhoneNumber.GetHashCode();
                if (this.NewPhoneNumber != null)
                    hashCode = hashCode * 59 + this.NewPhoneNumber.GetHashCode();
                if (this.Type != null)
                    hashCode = hashCode * 59 + this.Type.GetHashCode();
                return hashCode;
            }
        }
    }
}